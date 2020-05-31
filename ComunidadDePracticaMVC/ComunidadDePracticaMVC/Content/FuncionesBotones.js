

var btn0 = document.getElementById('b0');
var btn1 = document.getElementById('b1');
var btn2 = document.getElementById('b2');
var verValor = document.getElementById('pp');

//btn2.addEventListener('click', activarOtros);
//btn1.addEventListener('click', activarOtros);
//btn0.addEventListener('click', activarOtros);

function botonesHabilitados(elemento, event) {
    let valor;
    if (elemento != null) {
        valor = elemento.toString();
        if (valor == '1') {
            btn0.disabled = true;
            btn1.disabled = false;
            btn2.disabled = false;
        } else if (valor == '-1')
        {
            btn0.disabled = false;
            btn1.disabled = false;
            btn2.disabled = true;
        }
    }
}

function activarOtros(articuloID,valor) {
    var btn0 = document.getElementById('b0');
    var btn1 = document.getElementById('b1');
    var btn2 = document.getElementById('b2');

    if (valor === 1) {
        btn0.disabled = true;
        btn1.disabled = false;
        btn2.disabled = false;
        //alert(valor);
    } else if (valor === 0) {
        btn1.disabled = true;
        btn0.disabled = false;
        btn2.disabled = false;
        //alert(valor);
    } else if (valor === -1){
        btn2.disabled = true;
        btn1.disabled = false;
        btn0.disabled = false;
        //alert(valor);
    }
    enviarPuntos(articuloID, valor);
}

function enviarPuntos(articuloID, valor) {
    $(document).ready(function () {
        $.ajax(
            {
                //idS: $("ArtId").val(),
                type: "POST", //HTTP POST Method
                url: "Articulo/Puntuar",
                //contentType: "Application/json; charset=utf-8”,
                //dataType: "json",
                data: { //Passing data
                    Id: articuloID,
                    PuntajeLector: valor
                },

                success: function (data) {
                    alert(data);
                },

                failure: function (response) {
                    alert("gus bay F");
                },
                error: function (response) {
                    alert("gg F");
                }

            });
    });
}