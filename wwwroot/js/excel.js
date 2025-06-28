var tableToExcel = (function () {
    // Template HTML para o Excel
    let template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40">' +
        '<head><meta http-equiv="content-type" content="text/html; charset=UTF-8"/></head>' +
        '<body><table>{table}</table></body></html>';

    // Função para substituir as chaves do template
    function format(s, c) {
        return s.replace(/{(\w+)}/g, function (m, p) {
            return c[p];
        });
    }

    return function (table, name) {
        let ctx = {
            worksheet: name || 'Worksheet',
            table: table.innerHTML
        };

        try {
            // Gera o conteúdo formatado do Excel
            let formatted = format(template, ctx);

            // Cria um Blob com o conteúdo e o tipo MIME adequado
            let blob = new Blob([formatted], { type: "application/vnd.ms-excel" });

            // Cria uma URL para o Blob
            let url = URL.createObjectURL(blob);

            // Cria um elemento <a> e configura o download
            let a = document.createElement("a");
            a.href = url;
            a.download = name + '.xls';

            // Adiciona o elemento à página e simula um clique
            document.body.appendChild(a);
            a.click();

            // Remove o elemento e revoga a URL criada
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        } catch (e) {
            console.log('Erro ao exportar para Excel: ' + e);
        }
    }
})();

/**
    $('body').on('click', '#btnExcel', function (evt) {
        fn_gera_excel('tbl', 'nome-planilha');
    });
 */
function fn_gera_excel(tabelaID, pagina) {
    let rows = $(`#${tabelaID} tbody tr`).length;
    if (rows == 0) {
        Swal.fire({
            title: '<strong>Ops!</strong>',
            icon: 'info',
            html: `Não há dados para gerar o Excel.`,
            showCloseButton: false,
            showCancelButton: false,
            focusConfirm: false,
            confirmButtonText: 'Fechar',
            confirmButtonAriaLabel: 'Fechar'
        });
        return false;
    }

    let tableOri = document.getElementById(tabelaID);
    debugger;
    if (pagina == '' || pagina == undefined) {

        tableToExcel(tableOri, 'Relatorio');
    }
    else {
        tableToExcel(tableOri, pagina);
    }
}