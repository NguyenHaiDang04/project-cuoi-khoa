param(
    [switch]$SkipBuild,
    [string]$OutputDirectory = (Join-Path $PSScriptRoot 'TestOutput'),
    [string]$MSBuild
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Data

if (-not $SkipBuild) {
    if (-not $MSBuild) {
        $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
        if (Test-Path -LiteralPath $vswhere) {
            $MSBuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\Current\Bin\MSBuild.exe' |
                Select-Object -First 1
        }
    }
    if (-not $MSBuild) { throw 'Install Visual Studio with .NET desktop development.' }
    & $MSBuild (Join-Path $PSScriptRoot 'NovaCenter.sln') /t:Build /p:Configuration=Debug /v:minimal /nologo
    if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
}

$exe = Join-Path $PSScriptRoot 'NovaCenter\bin\Debug\NovaCenter.exe'
if (-not (Test-Path -LiteralPath $exe)) { throw 'Build the application before verification.' }
$taskOutput = [IO.Path]::GetFullPath($OutputDirectory)
[void][IO.Directory]::CreateDirectory($taskOutput)
$taskDb = 'Nova_Test_' + [guid]::NewGuid().ToString('N')
$oldConnection = [Environment]::GetEnvironmentVariable('NOVA_CONNECTION','Process')
try {
    $env:NOVA_CONNECTION = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=$taskDb;Integrated Security=True;Connect Timeout=15"
    $run = Start-Process -FilePath $exe -ArgumentList @('--verify',('"' + $taskOutput + '"')) -WindowStyle Hidden -PassThru
    try {
        if (-not $run.WaitForExit(180000)) {
            $run.Kill()
            $run.WaitForExit()
            throw 'Verification timed out.'
        }
        $code = $run.ExitCode
    } finally { $run.Dispose() }

    $log = Join-Path $taskOutput 'verification.txt'
    if (Test-Path -LiteralPath $log) { Get-Content -LiteralPath $log -Encoding UTF8 }
    if ($code -ne 0) { throw 'Verification failed. See verification.txt.' }
} finally {
    [Environment]::SetEnvironmentVariable('NOVA_CONNECTION',$oldConnection,'Process')
    if ($taskDb -notmatch '^Nova_Test_[0-9a-f]{32}$') { throw 'Invalid disposable database name.' }
    $connection = New-Object System.Data.SqlClient.SqlConnection(
        'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Pooling=False'
    )
    try {
        $connection.Open()
        $command = $connection.CreateCommand()
        try {
            $command.CommandText = "IF DB_ID(N'$taskDb') IS NOT NULL BEGIN ALTER DATABASE [$taskDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$taskDb]; END"
            [void]$command.ExecuteNonQuery()
        } finally { $command.Dispose() }
    } finally { $connection.Dispose() }
}
Write-Output ('Evidence saved to ' + $taskOutput)
