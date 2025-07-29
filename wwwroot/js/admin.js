
function changeView(view) {
    const contentArea = document.getElementById('content');
    contentArea.innerHTML = 'Đang tải...';
    const sidebar = document.getElementById('sidebar');

    $.ajax({
        
        url: `/Admin/${view}`, 
        type: 'GET',
        success: function (data) {
            contentArea.innerHTML = data;  
        },
        error: function () {
            contentArea.innerHTML = '<p>Đã có lỗi xảy ra, vui lòng thử lại sau.</p>'; 
        }

    });
}
function enableEditing() {
   
    document.getElementById('fullName').disabled = false;
    document.getElementById('phoneNumber').disabled = false;
    document.getElementById('email').disabled = false;
    document.getElementById('btnModify').disabled = true;
    document.getElementById('btnSave').disabled = false;
}

function enableSave() {

    document.getElementById('fullName').disabled = true;
    document.getElementById('phoneNumber').disabled = true;
    document.getElementById('email').disabled = true;
    document.getElementById('btnSave').disabled = true;
    document.getElementById('btnModify').disabled = false;
}
