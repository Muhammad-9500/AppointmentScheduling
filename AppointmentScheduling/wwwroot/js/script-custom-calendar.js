var routeUrl = location.protocol + "//" + location.host;
$(document).ready(function () {

    $("#appointmentDate").kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });

    InitializeCalendar(); 
});
var calendar;
function InitializeCalendar() {
    try
    {
             
        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                },
                eventDisplay:'block',
                events: function (fetchInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: routeUrl + '/Api/Appointment/GetCalendarData?doctorId=' + $("#doctorId").val(),
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (response) {
                            var events = [];
                            if (response.status === 1) {
                                $.each(response.dataEnum, function (i,data) {
                                    events.push(
                                        {
                                            title: data.title,
                                            description: data.description,
                                            start: data.startDate,
                                            end: data.endDate,
                                            backgroundColor: data.isDoctorApproved ? "#28a745" : "#dc3545",
                                            borderColor: "#162466",
                                            textColor: "white",
                                            id:data.id
                                        });
                                })
                            }
                            successCallback(events);
                        },
                        error: function (xhr) {
                            toastr.error('An error occured in the solution!', 'error')
                        }
                    });
                },
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event);

                }

            });
            calendar.render();
        }

    }
    catch (e)
    {
        alert(e)
    }
}

function onShowModal(obj, isEventDetail) {
    if (isEventDetail != null) {

        $("#title").val(obj.title);
        $("#descriptions").val(obj.description);
        $("#appointmentDate").val(obj.startDate);
        $("#duration").val(obj.duration);
        $("#doctorId").val(obj.doctorId);
        $("#patientId").val(obj.patientId);
        $("#id").val(obj.id);


        $("#lblPatientName").html(obj.patientName);
        $("#lblDoctorName").html(obj.doctorName);

        if (obj.isDoctorApproved) {
            $("#lblStatus").html('Approved!');
            $("#btnConfirm").addClass("d-none");
            $("#btnSubmit").addClass("d-none");
        }
        else {
            $("#lblStatus").html('Pending!');
            $("#btnConfirm").removeClass("d-none");
            $("#btnSubmit").removeClass("d-none");
        }
        $("#btnDelete").removeClass("d-none");
    }
    else {
        $("#appointmentDate").val(obj.startStr + " " + new moment().format("hh:mm A"));
        $("#id").val(0);
        $("#btnDelete").addClass("d-none");
        $("#btnSubmit").removeClass("d-none");
    }
    $("#appointmentInput").modal("show");
    
}

function onCloseModal()
{
    $("#appointmentForm")[0].reset();
    $("#id").val(0);

    $("#title").val('');
    $("#descriptions").val('');
    $("#appointmentDate").val('');
    $("#duration").val('');
    
    $("#patientId").val('');

    $("#appointmentInput").modal("hide");
}

function onSubmitForm() {
    if (checkValidation()) {
        var requestData = {
            Id: parseInt($("#id").val()),
            Title: $("#title").val(),
            Description: $("#descriptions").val(),
            StartDate: $("#appointmentDate").val(),
            EndDate: $("#appointmentDate").val(),
            Duration: $("#duration").val(),
            DoctorId: $("#doctorId").val(),
            PatientId: $("#patientId").val(),
            DoctorName:'Aslam',
            PatientName:'Ayman',
            AdminName:'Zayn',
            AdminId:'e3rr4334678plplk'
        };

        $.ajax({
            url: routeUrl + '/Api/Appointment/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status === 1 || response.status === 2) {
                    calendar.refetchEvents();
                    toastr.success(response.message, 'success');
                    onCloseModal();
                }
                else {
                    console.log(response,response.Message);
                    toastr.warning(response.message, 'warning');
                }
                
            },
            error: function (xhr) {
                toastr.error('An error occured in the solution!', 'error')
            }

        });
    }

}

function checkValidation() {
    var isValid = true;
    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }

    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() === "") {
        isValid = false;
        $("#appointmentDate").addClass('error');
    }
    else {
        $("#appointmentDate").removeClass('error');
    }

    return isValid;
}


function getEventDetailsByEventId(info)
{
    $.ajax({
        url: routeUrl + '/Api/Appointment/GetCalendarDataById/'+info.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1 && response.dataEnum !== undefined) {
                onShowModal(response.dataEnum,true);
            }
            
        },
        error: function (xhr) {
            toastr.error('An error occured in the solution!', 'error')
        }
    });
}

function onDoctorChange(){
    calendar.refetchEvents();
}

function onDeleteAppointment() {
    var id = parseInt($("#id").val());
    $.ajax({
        url: routeUrl + '/Api/Appointment/DeleteAppointment/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1) {
                toastr.success(response.message, 'success');
                calendar.refetchEvents();
                onCloseModal();
            }
            else {
                toastr.warning(response.message, 'error');
            }
        },
        error: function (xhr) {
            toastr.error('Error!', 'error')
        }
    });

}

function onConfirmAppointment() {
    var id = parseInt($("#id").val());
    $.ajax({
        url: routeUrl + '/Api/Appointment/ConfirmEvent/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1) {
                toastr.success(response.message, 'success');
                calendar.refetchEvents();
                onCloseModal();
            }
            else {
                toastr.warning(response.message, 'error');
            }

        },
        error: function (xhr) {
            toastr.error('Error!', 'error')
        }
    });
}