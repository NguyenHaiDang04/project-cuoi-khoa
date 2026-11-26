# HƯỚNG DẪN THỰC HIỆN DỰ ÁN NOVA

## 1. Đọc tài liệu theo thứ tự

1. Đọc [README](../README.md) để biết đề tài, thành viên và cách chạy.
2. Đọc [phân tích yêu cầu](PHAN_TICH_YEU_CAU.md) để biết chức năng và quy tắc.
3. Làm theo tài liệu này để hiểu mã nguồn và phần việc của mình.
4. Dùng [kế hoạch kiểm thử](KE_HOACH_KIEM_THU.md) để kiểm tra trước khi ghép và nộp.

Bản mã nguồn hiện có là bản nền chạy được. Mỗi thành viên cần đọc, thực hành sửa và giải thích được phần phụ trách trước khi trình bày.

## 2. Chạy lần đầu

### Bước 1. Chuẩn bị máy

Cần Windows, Git, Visual Studio có workload **.NET desktop development**, .NET Framework 4.8 Developer Pack và SQL Server LocalDB.

Trong Visual Studio Installer, chọn **Modify**, kiểm tra workload và Developer Pack nếu mở solution báo thiếu framework.

### Bước 2. Tải đúng dự án

Mở terminal trong thư mục muốn lưu và chạy:

```powershell
git clone https://github.com/NguyenHaiDang04/project-cuoi-khoa.git
cd project-cuoi-khoa
```

Nếu đã clone trước đó, mở terminal trong thư mục dự án, kiểm tra thay đổi của mình bằng `git status`, commit phần đang làm trước khi cập nhật. Để cập nhật nhánh chính:

```powershell
git switch main
git pull --ff-only origin main
```

### Bước 3. Mở và build

1. Mở file `NovaCenter.sln`, không mở riêng một file `.cs`.
2. Trong Solution Explorer, nhấp phải project **NovaCenter** → **Set as Startup Project**.
3. Chọn **Build → Build Solution** hoặc nhấn **Ctrl+Shift+B**.
4. Đọc cửa sổ Error List nếu build lỗi. Chỉ chuyển sang bước tiếp theo khi build thành công.
5. Nhấn **F5** để chạy.

Lần đầu chương trình tự tạo `NovaCenterDB`, sáu bảng, dữ liệu mẫu và hai tài khoản mẫu. Tài khoản SQL kết nối cần quyền tạo CSDL khi CSDL chưa tồn tại.

### Bước 4. Đăng nhập

| Tài khoản | Mật khẩu mẫu | Dùng để thử |
|---|---|---|
| admin | Admin@123 | Toàn bộ chức năng, gồm quyền xóa |
| nhanvien | Nova@123 | Nhập, sửa, xem dữ liệu và kiểm tra hạn chế quyền xóa |

Mật khẩu mẫu được tạo ở lần đầu. Nếu đã đổi mật khẩu, những lần chạy sau dùng mật khẩu mới.

### Bước 5. Nếu không kết nối được CSDL

Mở [App.config](../NovaCenter/App.config). Chuỗi mặc định dùng `Data Source=(localdb)\MSSQLLocalDB` và `Initial Catalog=NovaCenterDB`.

- Kiểm tra máy đã cài và chạy được SQL Server LocalDB.
- Nếu dùng SQL Server khác, thay `Data Source` bằng đúng tên instance trên máy.
- Có thể kết nối bằng SSMS và chạy [ScriptDB.sql](../Database/ScriptDB.sql).
- Chạy ứng dụng để khởi tạo tài khoản mẫu, rồi thử đăng nhập lại.
- Không xóa CSDL đang dùng để xử lý lỗi kết nối. Kiểm tra server, tên CSDL và quyền kết nối trước.

Script SQL tạo bảng và dữ liệu mẫu. Phần tạo tài khoản có mật khẩu băm được thực hiện khi chạy ứng dụng.

## 3. Hiểu cách code hoạt động

Khi bấm **Lưu học viên**, chương trình đi qua các bước sau:

1. `SaveStudentClick` ở GUI đọc dữ liệu từ ô nhập.
2. GUI tạo đối tượng `Student` trong DTO.
3. `StudentService.SaveStudent` ở BUS kiểm tra mã, họ tên, ngày sinh, điện thoại và email.
4. `StudentRepository.SaveStudent` ở DAL chạy câu SQL có tham số.
5. Sau khi lưu thành công, GUI gọi `LoadAll` để nạp lại bảng và danh sách chọn.

| Thành phần | Hiểu đơn giản | File ví dụ |
|---|---|---|
| DTO | Gói thông tin của một học viên, khóa hoặc lớp | [Models.cs](../NovaCenter/DTO/Models.cs) |
| GUI | Ô nhập, bảng, nút và sự kiện click | [MainForm.Students.cs](../NovaCenter/GUI/MainForm.Students.cs) |
| BUS | Kiểm tra dữ liệu và điều kiện được phép thực hiện | [StudentService.cs](../NovaCenter/BUS/StudentService.cs) |
| DAL | Kết nối và đọc/ghi SQL Server | [StudentRepository.cs](../NovaCenter/DAL/StudentRepository.cs) |

`MainForm.Students.cs`, `MainForm.Training.cs` và `MainForm.Study.cs` là các phần của cùng lớp `MainForm`, được ghép bằng từ khóa `partial`. Bố cục control đặt trong `MainForm.Designer.cs`. Sự kiện nút và nghiệp vụ đặt trong các file module.

Khi sửa giao diện, đọc đúng tên control và hàm sự kiện đang được gán trong Designer. Cả nhóm thống nhất một người sửa file Designer chung trong mỗi lượt tích hợp.

## 4. Nguyễn Hải Đăng — đăng nhập và học viên

**Nhánh** `hoc-vien`

**Bảng** TaiKhoan và HocVien

### Các file cần đọc

| File | Cần hiểu và thực hiện |
|---|---|
| [LoginForm.cs](../NovaCenter/GUI/LoginForm.cs) | Nạp CSDL, xử lý đăng nhập và hiện/ẩn mật khẩu |
| [LoginForm.Designer.cs](../NovaCenter/GUI/LoginForm.Designer.cs) | Ô tài khoản, mật khẩu, nút đăng nhập/thoát và ErrorProvider |
| [AuthService.cs](../NovaCenter/BUS/AuthService.cs) | Kiểm tra tài khoản, phân quyền và đổi mật khẩu |
| [AccountRepository.cs](../NovaCenter/DAL/AccountRepository.cs) | Đọc tài khoản và cập nhật mật khẩu |
| [MainForm.Students.cs](../NovaCenter/GUI/MainForm.Students.cs) | Nhập mới, chọn dòng, lưu, xóa và tìm học viên |
| [StudentService.cs](../NovaCenter/BUS/StudentService.cs) | Kiểm tra thông tin học viên |
| [StudentRepository.cs](../NovaCenter/DAL/StudentRepository.cs) | SQL thêm/sửa/xóa/tìm học viên |
| [MainForm.Password.cs](../NovaCenter/GUI/MainForm.Password.cs) | Sự kiện đổi mật khẩu |

### Thứ tự thực hiện

1. Đọc lớp `Student` và `UserSession` trong DTO, đối chiếu cột HocVien/TaiKhoan trong CSDL.
2. Đọc và thử truy vấn danh sách học viên trong DAL.
3. Theo dõi thao tác **Nhập mới → điền thông tin → Lưu học viên**.
4. Chọn lại dòng vừa thêm để sửa, sau đó tìm theo mã hoặc tên.
5. Thử xóa học viên chưa đăng ký lớp và học viên đã có đăng ký.
6. Thử đăng nhập đúng/sai, hiện/ẩn mật khẩu và đổi mật khẩu.
7. Kiểm tra cùng chức năng bằng tài khoản nhân viên.

### Khi nào phần việc đạt

- Tài khoản trống hoặc sai không mở được màn hình chính.
- Học viên hợp lệ lưu và nạp lại được, mã trùng bị chặn.
- Ngày sinh, điện thoại và email sai được báo lỗi.
- Nhân viên không xóa được dữ liệu.
- Giải thích được vì sao SQL dùng tham số và mật khẩu được băm.
- Có kết quả kiểm tra các mã T01–T07 và T19 trong tài liệu kiểm thử.

## 5. Đoàn Thành Đạt — khóa học và lớp học

**Nhánh** `khoa-lop`

**Bảng** KhoaHoc và LopHoc

### Các file cần đọc

| File | Cần hiểu và thực hiện |
|---|---|
| [MainForm.Training.cs](../NovaCenter/GUI/MainForm.Training.cs) | Nhập mới, chọn dòng, lưu và xóa khóa/lớp |
| [TrainingService.cs](../NovaCenter/BUS/TrainingService.cs) | Kiểm tra mã, tên, số giờ, học phí, ngày học và sĩ số |
| [TrainingRepository.cs](../NovaCenter/DAL/TrainingRepository.cs) | SQL khóa/lớp và xử lý cập nhật lớp |
| [Models.cs](../NovaCenter/DTO/Models.cs) | Đối tượng Course và ClassRoom |

### Thứ tự thực hiện

1. Đọc bảng KhoaHoc trước, sau đó đọc khóa ngoại `LopHoc.KhoaHocId`.
2. Tạo và sửa một khóa học trên form.
3. Tạo lớp thuộc khóa vừa tạo, chọn giảng viên, thời gian và sĩ số.
4. Thử ngày kết thúc trước ngày bắt đầu, mã trùng và giá trị nhập sai.
5. Tạo đăng ký bằng module của Hải để thử giảm sĩ số nhỏ hơn số đã đăng ký.
6. Ngừng hoạt động khóa, kiểm tra cập nhật thông tin lớp cũ và chặn lớp mới.
7. Thử xóa lớp chưa có đăng ký, sau đó xóa khóa chưa có lớp.

### Khi nào phần việc đạt

- Khóa và lớp lưu đúng liên kết.
- Mã trùng, ngày sai, số giờ/sĩ số/học phí không hợp lệ bị chặn.
- Lớp có đăng ký giữ nguyên khóa học.
- Lớp cũ cập nhật được ngày/giảng viên khi khóa ngừng hoạt động.
- Xóa dữ liệu tuân theo quan hệ khóa ngoại.
- Có kết quả kiểm tra các mã T08–T12.

## 6. Ngô Hoàng Hải — đăng ký, điểm và thống kê

**Nhánh** `dang-ky-diem`

**Bảng** DangKy và KetQua

### Các file cần đọc

| File | Cần hiểu và thực hiện |
|---|---|
| [MainForm.Study.cs](../NovaCenter/GUI/MainForm.Study.cs) | Chọn học viên/lớp, đăng ký, nhập điểm và nạp thống kê |
| [StudyService.cs](../NovaCenter/BUS/StudyService.cs) | Điều kiện đăng ký, trạng thái và điểm |
| [StudyRepository.cs](../NovaCenter/DAL/StudyRepository.cs) | Lưu đăng ký, giao dịch, kết quả và truy vấn thống kê |
| [CsvExporter.cs](../NovaCenter/BUS/CsvExporter.cs) | Ghi tệp CSV có tiếng Việt |

### Thứ tự thực hiện

1. Lấy danh sách học viên và lớp từ hai module trước.
2. Chọn học viên/lớp rồi lưu đăng ký.
3. Thử ghi danh lại cùng học viên và thử một học viên khác vào lớp đầy.
4. Cập nhật trạng thái, hủy và khôi phục đăng ký.
5. Chọn đúng dòng trong tab kết quả, nhập hai điểm và lưu.
6. Đối chiếu điểm TB, xếp loại và số liệu thống kê.
7. Xuất CSV rồi mở tệp bằng Excel hoặc trình đọc văn bản.

### Khi nào phần việc đạt

- Một học viên không bị ghi danh trùng trong cùng lớp.
- Kiểm tra số chỗ và ghi đăng ký trong cùng giao dịch.
- Đăng ký đã hủy được loại khỏi thống kê.
- Điểm 8 và 9 cho điểm TB 8,60 và xếp loại Đạt.
- Điểm ngoài 0–10 bị chặn.
- Học phí cũ giữ nguyên khi đổi học phí danh mục.
- Có kết quả kiểm tra các mã T13–T18.

## 7. Bài thực hành chung để ghép ba phần

Chạy trên CSDL thử hoặc thống nhất dùng dữ liệu demo. Nếu các mã dưới đây đã tồn tại, dùng mã mới tương đương.

| Dữ liệu | Giá trị mẫu |
|---|---|
| Học viên | HVDEMO, Nguyễn Minh Anh, sinh 15/05/2004, Nam, 0901234567 |
| Khóa | DEMO, Tin học thực hành, 30 giờ, 1.000.000 đ, đang hoạt động |
| Lớp | DEMO-01, khóa DEMO, Cô Lan, bắt đầu hôm nay, kết thúc sau 30 ngày, sĩ số 2, trạng thái Mở |
| Điểm | Quá trình 8, cuối kỳ 9 |

1. Đăng nhập admin.
2. Đăng tạo HVDEMO ở tab Học viên.
3. Đạt tạo khóa DEMO và lớp DEMO-01.
4. Hải đăng ký HVDEMO vào DEMO-01.
5. Thử đăng ký lại HVDEMO. Kết quả mong đợi là báo trùng.
6. Đăng ký một học viên mẫu khác vào lớp. Thử học viên thứ ba. Kết quả mong đợi là lớp đầy.
7. Lọc DEMO-01 ở tab kết quả, chọn HVDEMO và lưu điểm 8/9.
8. Kiểm tra điểm TB 8,60, xếp loại Đạt.
9. Xem thống kê DEMO-01. Có 2 đăng ký còn hiệu lực thì học phí đăng ký là 2.000.000 đ.
10. Xuất báo cáo CSV và kiểm tra tiếng Việt.

Lưu ý thao tác chọn dòng là bắt buộc trước khi sửa hoặc nhập điểm. Bấm **Nhập mới** khi muốn thêm, tránh sửa nhầm bản ghi đang chọn.

## 8. Làm việc với Git và ghép bài

Sau khi clone, chọn đúng nhánh. Ví dụ phần của Đạt:

```powershell
git switch khoa-lop
git status
```

Sau khi sửa và tự kiểm tra:

```powershell
git add NovaCenter/GUI/MainForm.Training.cs NovaCenter/BUS/TrainingService.cs NovaCenter/DAL/TrainingRepository.cs
git commit -m "Cap nhat module khoa hoc va lop hoc"
git push origin khoa-lop
```

Trên GitHub, tạo pull request từ nhánh của mình vào `main`. Một bạn khác đọc thay đổi và kiểm tra trước khi ghép. Các file chung phải được trao đổi trước. Không dùng lệnh ép ghi đè lịch sử để xử lý xung đột.

Sau khi một module khác đã được ghép, commit phần đang làm rồi cập nhật:

```powershell
git fetch origin
git merge origin/main
```

Nếu Git báo conflict, dừng sửa phần đang lỗi, cùng người phụ trách file đó đối chiếu hai thay đổi, sửa nội dung đúng rồi build lại trước khi commit.

Quyền cộng tác viên phải được chủ repo cấp và người được mời chấp nhận. Có tên trong bảng phân công chưa tự tạo quyền ghi trên GitHub.

## 9. Hồ sơ bàn giao của từng người

Mỗi người gửi lại cho nhóm:

1. Commit hoặc pull request của phần mình.
2. Danh sách chức năng đã thử và dữ liệu đã dùng.
3. Ảnh minh họa một thao tác thành công và một trường hợp báo lỗi.
4. Những mục chưa kiểm tra hoặc còn lỗi.
5. Phần trình bày ngắn về luồng GUI → BUS → DAL của một chức năng.

Nhóm dùng [bảng kiểm thử](KE_HOACH_KIEM_THU.md) để ghi kết quả thật. Các mục thử tay chỉ được ghi Đạt sau khi đã thao tác và có bằng chứng.

## 10. Kiểm tra trước khi nộp

- Đủ tên, MSV, lớp và phân công ba người.
- README mở được link, ảnh và hướng dẫn chạy.
- Mở solution, build thành công, đăng nhập được.
- Hoàn thành bài thực hành chung ở mục 7.
- Chạy `Verify.ps1`, kiểm tra dòng kết quả cuối.
- ZIP có solution, mã nguồn, Database, docs và README.
- ZIP loại thư mục .git, .vs, bin, obj, TestOutput và tệp dữ liệu CSDL.
- Cả ba người giải thích được phần mình và cách các bảng liên kết.
