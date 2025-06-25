# 🛒 LOMS - Livestream Order Management System
![Android](https://img.shields.io/badge/Platform-Android-green) 
![Firebase](https://img.shields.io/badge/Backend-Firebase-orange) 
![NLP](https://img.shields.io/badge/AI-NLP_Comment_Parsing-blueviolet)

<div align="center">
  <img src="https://github.com/[your-username]/LOMS/blob/main/screenshots/app_demo.gif?raw=true" width="300" alt="LOMS in Action">
  <p><i>Real-time order processing during livestream</i></p>
</div>

## 🌟 Giới thiệu
**LOMS** là ứng dụng Android hỗ trợ bán hàng livestream trên nền tảng MXH (Facebook/TikTok), giải quyết 4 thách thức lớn:
1. Quá tải bình luận đặt hàng
2. Quản lý đa nhiệm giữa livestreamer - supporter - admin
3. Kiểm soát tồn kho thời gian thực
4. Phân loại khách hàng thông minh

## 🚀 Tính năng nổi bật
| Chức năng | Công nghệ | Lợi ích |
|-----------|-----------|---------|
| **AI Comment Filter** | NLP Python Microservice | Giảm 80% bình luận thủ công |
| **Multi-Role Dashboard** | Firebase Realtime DB | Đồng bộ đơn hàng xuyên vai trò |
| **Smart Waitlist** | Android WorkManager | Tự động nhắc nhở thanh toán |
| **1-Click Reorder** | Room Database | Tăng tốc độ đặt hàng cũ |

## 📸 Hình ảnh demo
<div align="center">
  <img src="https://github.com/[your-username]/LOMS/blob/main/screenshots/livestreamer_view.png?raw=true" width="30%" alt="Livestreamer View">
  <img src="https://github.com/[your-username]/LOMS/blob/main/screenshots/inventory_alert.png?raw=true" width="30%" alt="Low Stock Alert"> 
  <img src="https://github.com/[your-username]/LOMS/blob/main/screenshots/admin_report.png?raw=true" width="30%" alt="Sales Report">
</div>

## 🛠 Cài đặt
1. **Yêu cầu hệ thống**:
   - Android Studio Flamingo+
   - Firebase project (kèm file `google-services.json`)

2. **Chạy ứng dụng**:
```bash
git clone https://github.com/[your-username]/LOMS.git
cd LOMS
# Mở bằng Android Studio
