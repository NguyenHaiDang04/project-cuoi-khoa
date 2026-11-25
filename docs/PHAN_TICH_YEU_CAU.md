# PHÂN TÍCH YÊU CẦU ĐỀ TÀI NOVA

## 1. Bài toán

Trung tâm có các khóa tin học. Mỗi khóa mở nhiều lớp. Học viên đăng ký lớp và được lưu kết quả sau khi học. Nhân viên cần tra cứu danh sách, cập nhật thông tin và tổng hợp số liệu theo lớp.

Ứng dụng được thực hiện bằng WinForms, dùng SQL Server để lưu dữ liệu. Phạm vi phù hợp với nhóm ba sinh viên, mỗi người phụ trách hai bảng và module liên quan.

## 2. Người dùng

| Người dùng | Quyền sử dụng |
|---|---|
| Quản trị viên | Đăng nhập, quản lý dữ liệu đào tạo, xóa bản ghi hợp lệ và đổi mật khẩu |
| Nhân viên | Đăng nhập, thêm/sửa/xem dữ liệu đào tạo, xuất CSV và đổi mật khẩu |

## 3. Yêu cầu chức năng và điều kiện chấp nhận

| Mã | Yêu cầu | Điều kiện kiểm tra |
|---|---|---|
| YC01 | Đăng nhập | Chặn trường trống và sai mật khẩu, tài khoản đúng mở màn hình chính |
| YC02 | Quản lý học viên | Thêm/sửa/xóa bản ghi hợp lệ, tìm theo mã/tên/điện thoại và chặn trùng mã |
| YC03 | Quản lý khóa | Lưu mã, tên, thời lượng, học phí và trạng thái, số giờ dương và học phí không âm |
| YC04 | Quản lý lớp | Chọn khóa hợp lệ, nhập giảng viên, ngày bắt đầu/kết thúc, sĩ số và trạng thái |
| YC05 | Đăng ký học | Lưu học viên vào lớp, chặn trùng và lớp đầy, cập nhật trạng thái đăng ký |
| YC06 | Quản lý kết quả | Điểm từ 0 đến 10, điểm TB đúng công thức và xếp loại đúng |
| YC07 | Thống kê | Tổng hợp theo lớp, loại đăng ký đã hủy, giữ học phí tại thời điểm ghi danh |
| YC08 | Xuất CSV | Xuất danh sách học viên, bảng điểm và báo cáo, đọc được tiếng Việt trong Excel |
| YC09 | Đổi mật khẩu | Kiểm tra mật khẩu hiện tại, xác nhận khớp và giữ mật khẩu mới sau khi khởi động lại |

## 4. Luồng nghiệp vụ chính

1. Quản trị viên hoặc nhân viên đăng nhập.
2. Nhập thông tin học viên.
3. Tạo khóa học và mở lớp thuộc khóa.
4. Chọn học viên và lớp để đăng ký.
5. Khi có kết quả học, chọn đăng ký và nhập điểm.
6. Xem danh sách theo lớp, thống kê và xuất báo cáo CSV.

## 5. Quy tắc dữ liệu

- TaiKhoan có tên đăng nhập duy nhất. Mật khẩu được băm trước khi lưu.
- HocVien, KhoaHoc và LopHoc có mã riêng, không trùng trong từng bảng.
- Mỗi lớp thuộc một khóa. Ngày kết thúc từ ngày bắt đầu trở đi và sĩ số tối đa lớn hơn 0.
- Một học viên có tối đa một dòng đăng ký cho mỗi lớp, kể cả đăng ký đã hủy.
- Khi khôi phục đăng ký, chương trình kiểm tra lại trạng thái học viên, khóa/lớp và số chỗ.
- Đăng ký có kết quả cần xóa kết quả trước khi hủy. Bản ghi đang được tham chiếu được bảo vệ bằng khóa ngoại.
- Lớp đã có đăng ký giữ nguyên khóa học. Lớp cũ thuộc khóa ngừng hoạt động vẫn cập nhật được giảng viên, ngày học và trạng thái.
- Học phí được sao chép vào đăng ký khi ghi danh để giữ lịch sử.
- Nhóm quy ước điểm TB = 0,4 × điểm quá trình + 0,6 × điểm cuối kỳ, làm tròn hai chữ số. Điểm TB từ 5 là Đạt.
- Thống kê học phí là giá trị các đăng ký, không bao gồm quản lý phiếu thu hoặc công nợ.

## 6. Thiết kế dữ liệu

| Bảng | Khóa chính | Liên kết |
|---|---|---|
| TaiKhoan | Id | Bảng đăng nhập độc lập |
| HocVien | Id | Một học viên có nhiều đăng ký |
| KhoaHoc | Id | Một khóa có nhiều lớp |
| LopHoc | Id | KhoaHocId tham chiếu KhoaHoc |
| DangKy | Id | HocVienId và LopHocId tham chiếu học viên/lớp |
| KetQua | DangKyId | Mỗi đăng ký có tối đa một kết quả |

Script có dữ liệu mẫu để giảng viên chạy thử. [ScriptDB.sql](../Database/ScriptDB.sql) dùng trong SSMS, [Schema.sql](../Database/Schema.sql) được nhúng trong chương trình.

## 7. Phân lớp mã nguồn

| Tầng | Nhiệm vụ | Ví dụ |
|---|---|---|
| DTO | Chứa dữ liệu trao đổi | Student, Course, ClassRoom, UserSession |
| DAL | Kết nối và truy vấn SQL có tham số | StudentRepository, TrainingRepository, StudyRepository |
| BUS | Kiểm tra dữ liệu và nghiệp vụ | StudentService, TrainingService, StudyService |
| GUI | Nạp danh sách, nhận dữ liệu và xử lý nút | LoginForm, MainForm và các phần module |

Các chức năng đăng ký dùng giao dịch để kiểm tra số chỗ và lưu cùng một lượt.

## 8. Phân công và tích hợp

- Nguyễn Hải Đăng phụ trách TaiKhoan/HocVien, đăng nhập, đổi mật khẩu và quản lý học viên.
- Đoàn Thành Đạt phụ trách KhoaHoc/LopHoc, quản lý khóa, lớp và kiểm tra dữ liệu lớp.
- Ngô Hoàng Hải phụ trách DangKy/KetQua, đăng ký, điểm, thống kê và xuất CSV.
- Cả nhóm thiết kế CSDL, thống nhất phần dùng chung, ghép bài và kiểm tra luồng hoàn chỉnh.

## 9. Cách kiểm tra khi nộp

1. Giải nén toàn bộ mã nguồn hoặc clone repo.
2. Mở solution và chạy bằng Visual Studio.
3. Đăng nhập bằng tài khoản mẫu trong README.
4. Thêm một học viên, tạo khóa và lớp, sau đó đăng ký học viên vào lớp.
5. Thử đăng ký trùng, nhập sai ngày hoặc đăng ký vào lớp đầy.
6. Nhập điểm và đối chiếu thống kê, xuất CSV.
7. Đăng nhập nhân viên để kiểm tra quyền xóa.
8. Chạy `Verify.ps1` nếu cần kiểm tra lại tự động.

Đối chiếu [README](../README.md) và [nhật ký kiểm thử](images/verification.txt) để xem cách chạy và kết quả đã thực hiện.
