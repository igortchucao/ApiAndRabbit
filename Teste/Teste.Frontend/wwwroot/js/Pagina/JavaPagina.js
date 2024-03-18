$(document).ready(function () {
    
    $('#btnPreecherRabbit').click(function () {
        $.ajax({
            url: 'https://localhost:44311/api/beneficiario/rabbit/preencher',
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
                Swal.fire({
                    type: result.Type,
                    title: result.Title,
                    text: result.Message,
                }).then(function () {
                    table.draw();
                });
            }
        });
                
    });

    $('#btnLerRabbit').click(function () {
        $.ajax({
            url: 'https://localhost:44311/api/beneficiario/rabbit/preencher',
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
                Swal.fire({
                    type: result.Type,
                    title: result.Title,
                    text: result.Message,
                }).then(function () {
                    table.draw();
                });
            }
        });
    });

    $('#btnLerApi').click(function () {
        $.ajax({
            url: 'https://localhost:44311/api/beneficiario/rabbit/preencher',
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
                Swal.fire({
                    type: result.Type,
                    title: result.Title,
                    text: result.Message,
                }).then(function () {
                    table.draw();
                });
            }
        });
    });
});