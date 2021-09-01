$(document).ready(function () {
    page_initialize();

});

//Getting input values
function page_initialize() {
    var url = new URL(window.location.href);
    var event_id = url.searchParams.get("event_id");
    var resultsid = url.searchParams.get("resultsid");
    var channelid = url.searchParams.get("channelid");
    var f = url.searchParams.get("f");
    var t = url.searchParams.get("t");
    if (event_id == "" || resultsid == "" || channelid == "") {
        window.location = "";
    }
    else {
        get_register_success_page(event_id, resultsid, channelid, f, t);
    }
}
//Ajax call

function get_register_success_page(event_id, resultsid, channelid, f, t, offset_value, record_limit) {
    var main_obj = { event_id: event_id, resultsid: resultsid, channelid: channelid, f: f, t: t, offset_value: offset_value, record_limit: record_limit }
    var send_data = JSON.stringify(main_obj);
    console.log(send_data);
    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/get_register_success_page/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {
            console.log(response);
            try {
                if (response.StatusCode == 200) {
                    if (response.Data.length == 0) {
                        $('#success_datatable').hide();
                        $('#no_data_div').show();
                    }
                    else {
                        for (var i = 0; i < response.Data.length; i++) {
                            var thumbnail_image_html = "";
                            for (var j = 0; j < response.Data[i].ThumbnailImage.length; j++) {
                                thumbnail_image_html += '<img style="width:100px; height:100px; border-radius:50%" src="data:image/jpeg;base64,' + response.Data[i].ThumbnailImage[j] + '" onclick="ModalboxImage(this)" />';
                            }
                            $('#success_datatable tbody').append('<tr><td>' + response.Data[i].CRN + '</td> <td>' + response.Data[i].CreatedOn + '</td> <td>' + response.Data[i].ResulstReason + '</td> <td>' + response.Data[i].Version + '</td> <td>' + thumbnail_image_html + ' </td> <td>' + response.Data[i].DeviceDetails + '</td></tr>');
                        }
                        $('#success_datatable').show();
                        $('#no_data_div').hide();
                    }
                }
                else {
                    $('#success_datatable').show();
                    $('#no_data_div').hide();
                }
            }
            catch (err) {
                console.log(err);
                $('#success_datatable').show();
                $('#no_data_div').hide();
            }
        },
        error: function (response) {
            //window.location = '/WW/Manage_Surveys/';
            $('#success_datatable').show();
            $('#no_data_div').hide();
        }
    });

}

function ModalboxImage(element) {
    var image = element.getAttribute("src");
    let html = '<div class="modal show" id="alert_popup" role="dialog" data-keyboard="false" data-backdrop="static"><div class="modal-dialog"><div class="modal-content"><div style="display:table-cell; vertical-align:middle; text-align:center;" class="modal-body"><img style="max-width: 375px;" src="' + image + '"><br><br><center><button type="button" id="modalButton" class="btn btn-primary"> Close </button></center></div></div></div></div>';

    document.getElementById("alert_popup_div").innerHTML = html;
    $('#alert_popup').modal('show');
    $("#modalButton").click(function () {
        $('#alert_popup').modal('hide');
    });
}


