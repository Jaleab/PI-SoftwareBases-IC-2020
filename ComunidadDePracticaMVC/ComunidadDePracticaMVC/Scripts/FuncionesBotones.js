

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
        if (valor === '1') {
            btn0.disabled = true;
            btn1.disabled = false;
            btn2.disabled = false;
        } else if (valor === '-1') {
            btn0.disabled = false;
            btn1.disabled = false;
            btn2.disabled = true;
        } else if (valor === '0') {
            btn0.disabled = false;
            btn1.disabled = true;
            btn2.disabled = false;
        }
    }
}

function activarOtros(articuloID, valor, correoUsuario, reaccionPasada) {
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
    var email = correoUsuario.toString();
    //alert(email);
    enviarPuntos(articuloID, valor, email, reaccionPasada);
}

function enviarPuntos(articuloID, valor,correoUsuario,reaccionPasada) {
    $.ajax({
        type: "GET",
        url: '/Articulo/Puntuar/',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { id: articuloID, puntaje: valor, correo: correoUsuario, reaccionVieja: reaccionPasada},
        success: function (response) {
            alert(response.message)
        },
        failure: function () {
            alert("Error al puntuar")
        },
        error: function (err) {
            alert("Error al puntuar")
        }

    });
}