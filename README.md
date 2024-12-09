

# 📄 **Hệ Thống WebService quản lý bán vé xem phim online - Môn học Công Nghệ .NET**

## 📖 **Giới thiệu**
Đây là dự án .NET được thực hiện trong khuôn khổ môn học Công Nghệ .NET(841467). Dự án này được phát triển bằng .NET (C#) và hướng tới mục đích xây dựng 1 WebService quản lý 1 rạp chiếu phim bán vé xem phim online.

Dự án được thiết kế và tổ chức theo mô hình **Clean Architecture** và **Microservices**, giúp hệ thống đảm bảo tính **modular, dễ bảo trì, mở rộng và tối ưu hóa hiệu suất**.

## 🛠️ **Công nghệ sử dụng**
- __Ngôn ngữ__:

  [![My Skills](https://skillicons.dev/icons?i=js,cs)](https://skillicons.dev)

- __Framework__:

  [![My Skills](https://skillicons.dev/icons?i=react,dotnet)](https://skillicons.dev)

- __Cơ sở dữ liệu__:

  [![My Skills](https://skillicons.dev/icons?i=mongo)](https://skillicons.dev)

- __IDE__:

  [![My Skills](https://skillicons.dev/icons?i=rider,vscode)](https://skillicons.dev)

- __Các công cụ hỗ trợ nghiệp vụ__:

  [![My Skills](https://skillicons.dev/icons?i=rabbitmq,docker,yarn,postman)](https://skillicons.dev)

- __API Gateway__ ![YARP]()

- __Service Discovery__ ![HashiCorp Consul]()

## 🚀 **Mục tiêu của đồ án**

Mục tiêu của dự án WebService quản lý bán vé xem phim online cho rạp phim là xây dựng một hệ thống backend mạnh mẽ, hỗ trợ các chức năng chính sau:

__1. Cho phép người dùng đặt vé trước:__

- Người dùng có thể tìm kiếm thông tin về các bộ phim và lịch chiếu.
- Dễ dàng chọn lịch chiếu và ghế để đặt vé trực tuyến.

__2. Quản lý thông tin các bộ phim:__

- Cập nhật thông tin về các bộ phim đang chiếu tại rạp.
- Thêm, sửa, xóa thông tin bộ phim từ phía hệ thống quản trị.

__3. Quản lý thông tin các lịch chiếu của rạp phim:__

- Thiết lập và cập nhật thông tin lịch chiếu theo từng thời điểm.
- Cung cấp thông tin về các suất chiếu và lịch trình phù hợp để người dùng đặt vé.

__4. Tối ưu hóa trải nghiệm người dùng:__

- Dựng mô hình dữ liệu và API để các chức năng diễn ra nhanh chóng và dễ dàng thông qua các endpoint được thiết kế tối ưu và bảo mật.

__5. Xây dựng mô hình API linh hoạt và dễ mở rộng:__

- Triển khai các endpoint __RESTful__ với chuẩn thiết kế tốt để dễ dàng mở rộng trong tương lai.
- Sử dụng các công cụ như __YARP__ và __HashiCorp Consul__ để tối ưu hóa việc load balancing và khám phá dịch vụ.


## 🏗️ **Cài đặt và chạy dự án**

__1. Yêu cầu hệ thống:__
- Hệ điều hành Windows 10, 11, Linux, MacOS.
- Cài đặt Docker và .NET.

__2. Clone dự án:__

```bash
  git clone https://github.com/ducphu2813/DotNet-Project.git
```

__3. Build và khởi động dự án:__

```bash
  docker-compose build
  docker-compose up
```

__4. Test APIs:__

- Import file __TheaterManagement.postman_collection.json__ vào Postman để xem tài liệu APIs.

## ✏️ **Giảng viên và nhóm thực hiện**

- __Giảng viên bộ môn:__ TỪ LÃNG PHIÊU

- __Danh sách thành viên:__

| Mã số sinh viên |  Họ và Tên  |
|:----------------|:--------:|
| 3120560104 | **Huỳnh Thế Vinh** |
| 3121410022 | **Nguyễn Đức Trí** |
| 3121410386 | **Tạ Đức Phú** |
| 3121410109 | **Mai Nguyễn Quốc Dũng** |
| 3121410024 | **Huỳnh Nhựt Dương** |
| 3121410015 | **Trần Phan Minh Thông** |