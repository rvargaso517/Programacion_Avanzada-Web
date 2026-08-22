document.addEventListener('DOMContentLoaded', function () {

    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {

        initialView: 'dayGridMonth',

        locale: 'es',

        height: 700,

        events: '/Citas/ObtenerCitas',

        eventClick: function(info) {
            try {
                if (info.jsEvent) {
                    info.jsEvent.preventDefault();
                }
                var event = info.event;
                
                var tituloEl = document.getElementById('detalleTitulo');
                if (tituloEl) tituloEl.innerText = event.title || 'Sin Título';
                
                var props = event.extendedProps || {};
                
                var clienteEl = document.getElementById('detalleCliente');
                if (clienteEl) clienteEl.innerText = props.cliente || 'No asignado';
                
                var entrenadorEl = document.getElementById('detalleEntrenador');
                if (entrenadorEl) entrenadorEl.innerText = props.entrenador || 'No asignado';
                
                var options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
                var startStr = event.start ? event.start.toLocaleDateString('es-ES', options) : 'Sin fecha';
                var fechaEl = document.getElementById('detalleFechaHora');
                if (fechaEl) fechaEl.innerText = startStr;
                
                var descEl = document.getElementById('detalleDescripcion');
                if (descEl) descEl.innerText = props.descripcion || 'Sin descripción';
                
                var estadoBadge = document.getElementById('detalleEstado');
                if (estadoBadge) {
                    var estado = props.estado || 'Pendiente';
                    estadoBadge.innerText = estado;
                    estadoBadge.className = 'badge ';
                    if (estado === 'Completada' || estado === 'Confirmada') {
                        estadoBadge.className += 'bg-success text-white';
                    } else if (estado === 'Cancelada' || estado === 'Inactivo') {
                        estadoBadge.className += 'bg-danger text-white';
                    } else {
                        estadoBadge.className += 'bg-warning text-dark';
                    }
                }
                
                var modalEl = document.getElementById('modalDetalleCita');
                if (modalEl) {
                    if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
                        var myModal = bootstrap.Modal.getOrCreateInstance(modalEl);
                        myModal.show();
                    } else {
                        alert("Detalles de la Cita:\n" +
                              "Título: " + (event.title || 'Sin Título') + "\n" +
                              "Fecha: " + startStr + "\n" +
                              "Cliente: " + (props.cliente || 'No asignado') + "\n" +
                              "Entrenador: " + (props.entrenador || 'No asignado') + "\n" +
                              "Descripción: " + (props.descripcion || 'Sin descripción') + "\n" +
                              "Estado: " + (props.estado || 'Pendiente'));
                    }
                }
            } catch (e) {
                console.error("Error en eventClick:", e);
                alert("Error de JS al abrir detalles: " + e.message);
            }
        }

    });

    calendar.render();

});