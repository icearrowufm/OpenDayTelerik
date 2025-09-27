// Thiết lập ngày đích
const targetDate = new Date("2024-11-03T07:00:00").getTime();

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
    const showcountdown = document.getElementById("showcountdown");
    const showdangky = document.getElementById("showdangky");
    // Nếu thời gian đã kết thúc, dừng đồng hồ đếm ngược
    if (distance < 0) {
        clearInterval(countdownInterval);
        showcountdown.style.display = "none";
        showdangky.style.display = "block";
    }
}, 1000);
