// Thiết lập ngày đích
const targetDate = new Date("2024-11-03T00:00:00").getTime();

// Cập nhật đồng hồ đếm ngược mỗi giây
const countdownInterval = setInterval(() => {
    // Lấy thời gian hiện tại
    const now = new Date().getTime();

    // Tính toán thời gian còn lại
    const distance = targetDate - now;

    // Tính toán ngày, giờ, phút và giây còn lại
    const days = Math.floor(distance / (1000 * 60 * 60 * 24));
    const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((distance % (1000 * 60)) / 1000);

    // Hiển thị kết quả trong phần tử countdown
    document.getElementById("countdown").innerHTML =
        days + " ngày " + hours + " giờ " + minutes + " phút " + seconds + " giây ";

    // Nếu thời gian đã kết thúc, dừng đồng hồ đếm ngược
    if (distance < 0) {
        clearInterval(countdownInterval);
        document.getElementById("countdown").innerHTML = "Đếm ngược đã kết thúc!";
    }
}, 1000);
