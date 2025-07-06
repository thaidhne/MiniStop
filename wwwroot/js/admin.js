function loadPage(page) {
    const content = document.getElementById('main-content');
    content.innerHTML = 'Đang tải...';

    fetch(`pages/${page}.html`)
        .then(res => {
            if (!res.ok) throw new Error("Không tìm thấy nội dung");
            return res.text();
        })
        .then(html => {
            content.innerHTML = html;
        })
        .catch(err => {
            content.innerHTML = `<p style="color:red;">Lỗi: ${err.message}</p>`;
        });
}
