function loadTopics(moduloId) {
    $.get(`/Topico/GetByModuloId/${moduloId}`, function (data) {
        $('#topics-container').html(data);
    }).fail(function () {
        alert("Erro ao carregar os tópicos.");
    });
}
