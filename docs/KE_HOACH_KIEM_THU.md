# KẾ HOẠCH KIỂM THỬ VÀ TIÊU CHÍ HOÀN THÀNH

## 1. Cách đọc kết quả

Tài liệu này đối chiếu yêu cầu nhóm đã thống nhất, không phải thang điểm chính thức của giảng viên.

- **Tự động** nghĩa là có ca trong [Verification.cs](../NovaCenter/Tests/Verification.cs), kết quả thực hiện nằm ở [verification.txt](images/verification.txt).
- **Thử tay** nghĩa là cần thao tác trực tiếp theo bảng dưới, ghi ngày, người thử, dữ liệu và kết quả.
- Một dòng có thể chứa nhiều điều kiện. Dòng nào chưa có bằng chứng cho tất cả điều kiện thì chưa kết luận toàn bộ dòng Đạt.

45 ca tự động hiện có không thay thế các mục thử tay như hộp thoại xác nhận xóa, chọn nơi lưu tệp và chạy trên máy khác.

Phiên kiểm tra ngày **27/11/2026** đã build cấu hình Debug thành công, chạy trên Windows với .NET Framework 4.8 và SQL Server LocalDB. Kết quả **45/45 PASS**, tiến trình kết thúc mã 0. Các mục thử tay chưa được ghi nhận hoàn thành trong phiên này.

## 2. Chuẩn bị

1. Build solution.
2. Ưu tiên dùng CSDL thử riêng. Không xóa CSDL đang chứa dữ liệu cần giữ.
3. Nếu kiểm tra bằng `Verify.ps1`, script tự tạo `Nova_Test_...` rồi dọn CSDL đó.
4. Thử tay có thể dùng bộ dữ liệu HVDEMO/DEMO/DEMO-01 trong [hướng dẫn](HUONG_DAN_THUC_HIEN.md).
5. Ghi kết quả theo mẫu ở mục 5, không ghi đè nhật ký tự động bằng kết quả tự khai.

## 3. Ma trận kiểm tra chi tiết

| Mã | Phụ trách | Thao tác và dữ liệu | Kết quả mong đợi | Bằng chứng hiện có / phần cần thử |
|---|---|---|---|---|
| T01 | Đăng | Để trống cả hai ô đăng nhập, rồi nhập admin với mật khẩu sai | ErrorProvider báo thiếu; sai mật khẩu không vào màn hình chính | Tự động |
| T02 | Đăng | Đăng nhập admin đúng, bật/tắt hiện mật khẩu, dùng Enter/Esc | Đăng nhập đúng; mật khẩu đổi hiển thị; Enter/Esc gán đúng nút | Tự động kiểm tra đăng nhập, checkbox và thuộc tính gán nút. Thử tay phím thật |
| T03 | Đăng | Nhập HVDEMO đầy đủ, chọn lại dòng để sửa địa chỉ | Lưu được và nạp lại đúng thông tin | Tự động kiểm tra thêm/sửa học viên |
| T04 | Đăng | Thêm lại mã HV001 | Báo trùng, số học viên không tăng | Thử tay hoặc bổ sung ca tự động riêng |
| T05 | Đăng | Bỏ tên, nhập điện thoại abc, email sai, thử ngày sinh tương lai | Từng trường sai bị chặn; DateTimePicker giới hạn ngày | Thử tay từng trường. Có kiểm tra trong BUS/control, chưa có ca riêng cho mọi trường |
| T06 | Đăng | Tìm theo mã, tên và điện thoại, thử chuỗi giống lệnh SQL | Chỉ hiện bản ghi khớp, không làm đổi dữ liệu | Tự động kiểm tra tên và chuỗi SQL. Thử tay mã/điện thoại |
| T07 | Đăng | Xóa học viên chưa đăng ký, xóa học viên đã đăng ký, chọn No ở xác nhận | Xóa bản ghi độc lập; bản ghi liên quan bị chặn; chọn No giữ nguyên | Tự động kiểm tra DAL/BUS và khóa ngoại. Thử tay Yes/No của hộp thoại |
| T08 | Đạt | Thêm/sửa khóa với mã DEMO, 30 giờ, học phí 1.000.000 đ | Lưu đúng thông tin | Tự động kiểm tra thêm/sửa khóa |
| T09 | Đạt | Trùng mã khóa/lớp; thử số giờ 0, phí âm, sĩ số 0 | Chặn dữ liệu không hợp lệ; control có giới hạn nhập | Thử tay. Có ràng buộc BUS/SQL/control |
| T10 | Đạt | Tạo lớp thuộc DEMO, nhập ngày kết thúc trước ngày bắt đầu | Lớp hợp lệ lưu đúng khóa, ngày sai báo lỗi | Tự động |
| T11 | Đạt | Lớp có đăng ký, thử giảm sĩ số dưới số đã đăng ký hoặc đổi khóa | Báo lỗi và giữ thông tin lớp cũ | Thử tay. Có kiểm tra trong TrainingRepository |
| T12 | Đạt | Ngừng khóa, sửa giảng viên/ngày lớp cũ; thử tạo lớp mới; xóa lớp và khóa chưa dùng | Lớp cũ cập nhật được; lớp mới bị chặn; xóa đúng quan hệ | Tự động |
| T13 | Hải | Đăng ký HVDEMO, đăng ký lại cùng lớp | Lưu lần đầu và chặn trùng | Tự động |
| T14 | Hải | Lớp sĩ số 2, thêm người thứ ba; hai lượt cùng chiếm suất cuối | Không vượt sĩ số, lượt bị chặn không để lại đăng ký thừa | Tự động kiểm tra lớp đầy và tranh suất cuối |
| T15 | Hải | Hủy/khôi phục đăng ký; nhập điểm rồi thử hủy | Khôi phục khi hợp lệ, đăng ký có điểm bị chặn hủy | Tự động. Thử tay khôi phục khi lớp đã đầy hoặc kết thúc |
| T16 | Hải | Nhập 8/9, thử điểm ngoài 0–10; xóa điểm bằng admin | TB 8,60 và Đạt; điểm sai bị chặn; xóa điểm hợp lệ | Tự động kiểm tra 8/9, điểm >10 và xóa điểm. Thử tay mốc 0/10 và TB dưới 5 |
| T17 | Hải | Đổi học phí khóa sau đăng ký, xem báo cáo theo lớp | Đăng ký cũ giữ giá; tổng đúng; đăng ký hủy không tính vào tổng | Tự động kiểm tra giữ giá và báo cáo mẫu. Thử tay đối chiếu tổng sau hủy |
| T18 | Hải | Xuất CSV, chọn thư mục, mở tệp, thử hủy Save | Có tệp đúng tiếng Việt; bấm hủy không tạo tệp | Tự động kiểm tra nội dung bộ xuất và dấu nháy/dấu phẩy. Thử tay hộp thoại Save, hủy và mở Excel |
| T19 | Đăng | Đăng nhập nhân viên; đổi mật khẩu sai xác nhận rồi đổi đúng; chạy lại app | Nhân viên không xóa được; xác nhận sai bị chặn; mật khẩu mới còn hiệu lực | Tự động |
| T20 | Cả nhóm | Khởi tạo CSDL lần nữa sau khi có dữ liệu mới | Dữ liệu và mật khẩu đã đổi giữ nguyên | Tự động |
| T21 | Cả nhóm | Giải nén ZIP trên máy khác, cài đúng môi trường rồi F5 | Build và đăng nhập được, tài liệu đủ để chạy | Cần thử tay trên máy thứ hai |
| T22 | Cả nhóm | Theo toàn bộ luồng ở mục 7 tài liệu hướng dẫn | Học viên → khóa/lớp → đăng ký → điểm → thống kê khớp nhau | Tự động đi qua luồng tương ứng. Nhóm cần thực hành luồng demo khi thuyết trình |

## 4. Kết luận được phép ghi

- **Build đạt** khi MSBuild/Visual Studio kết thúc thành công.
- **Kiểm thử tự động đạt** khi log có đủ dòng PASS và dòng cuối RESULT: PASS, tiến trình kết thúc mã 0.
- **Thử tay đạt** khi đã ghi thao tác thực tế và bằng chứng cho mục đó.
- **Sẵn sàng bàn giao nội bộ** khi ba người chạy được module và các lỗi quan trọng đã xử lý.
- **Sẵn sàng nộp theo yêu cầu lớp** cần đối chiếu thêm hướng dẫn/rubric giảng viên nếu có, hoàn thiện T21 và các mục thử tay trong bảng.

Không quy đổi 45/45 ca tự động thành điểm thi hay kết luận mọi tình huống đều đã được kiểm tra.

## 5. Mẫu ghi kết quả thử tay

Sao chép bảng sau vào ghi chú hoặc bình luận pull request khi thực hiện.

| Mã | Ngày thử | Người thử | Dữ liệu/thao tác thực tế | Kết quả thực tế | Đạt/Chưa đạt | Bằng chứng |
|---|---|---|---|---|---|---|
| T… | … | … | … | … | … | Ảnh/tên tệp/commit |

Nếu chưa thử, ghi **Chưa thử**. Nếu lỗi, ghi cách lặp lại, kết quả mong đợi và gửi người phụ trách module sửa, sau đó thử lại ca bị lỗi.

## 6. Kiểm tra tự động

Từ thư mục chứa solution:

```powershell
.\Verify.ps1
```

Kết quả mới nằm trong `TestOutput`. Đối chiếu nhật ký, không chỉ nhìn thấy cửa sổ mở được. Dữ liệu chạy thử tự động tách riêng với `NovaCenterDB`.

Bộ kiểm tra hiện tại tạo ảnh các màn hình và kiểm tra dữ liệu SQL sau thao tác form hoặc gọi BUS. Một số thao tác xóa được kiểm tra tại BUS để tránh hộp thoại dừng tiến trình.
