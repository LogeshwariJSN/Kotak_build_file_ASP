$(document).ready(function () {
    var offset_value = 0;
    var record_limit = 10;
    page_initialize(offset_value, record_limit); 
});

//Getting input values
function page_initialize(offset_value,record_limit) {
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
        get_register_success_page(event_id, resultsid, channelid, f, t, offset_value, record_limit);
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
                        $('#success_datatable tbody').empty();
                        $('#success_datatable thead tr').empty();
                    }
                    else {
                        $('#success_datatable tbody').empty();
                        $('#success_datatable thead tr').empty();
                        $('#top_total_count').html(response.Data.total_records);
                        $('#bottom_total_count').html(response.Data.total_records);

                        //Dashboard Success Report Data
                        if (response.Result == "Success") {
                            $("#success_datatable thead tr").append("<th>CRN</th><th>Created Date</th><th>Result Reason</th><th>Version</th><th>Thumbnail Images</th><th>Device Details</th>");
                            for (var i = 0; i < response.Data.DashboardSuccessImageData.length; i++) {
                                var thumbnail_image_html = "";
                                for (var j = 0; j < response.Data.DashboardSuccessImageData[i].ThumbnailImage.length; j++) {
                                    thumbnail_image_html += '<img style="width:100px; height:100px; border-radius:50%" src="data:image/jpeg;base64,' + response.Data.DashboardSuccessImageData[i].ThumbnailImage[j] + '" onclick="ModalboxImage(this)" />';
                                }
                                $('#success_datatable tbody').append('<tr><td>' + response.Data.DashboardSuccessImageData[i].CRN + '</td> <td>' + response.Data.DashboardSuccessImageData[i].CreatedOn + '</td> <td>' + response.Data.DashboardSuccessImageData[i].ResulstReason + '</td> <td>' + response.Data.DashboardSuccessImageData[i].Version + '</td> <td>' + thumbnail_image_html + ' </td> <td>' + response.Data.DashboardSuccessImageData[i].DeviceDetails + '</td></tr>');
                            }
                        }
                        else {
                            //Dashboard Failure Report Data
                            $("#success_datatable thead tr").append("<th>CRN</th><th>Created Date</th><th>Result Reason</th><th>Version</th><th>Verify</th><th>Thumbnail Images</th><th>Failed At</th><th>Device Details</th>");
                            //Thumbnail Images
                            for (var i = 0; i < response.Data.DashboardSPecificResultData.length; i++) {
                                var thumbnail_image_html = "", verify_button_html = "", failure_at="", result_reason="";
                                var get_event = JSON.parse(send_data).event_id == "1" ? "Registration" : "Verification";
                                if (response.Data.DashboardSPecificResultData[i].ThumbnailImage != "" || response.Data.DashboardSPecificResultData[i].ThumbnailImage != null) {
                                    thumbnail_image_html = '<img style="width: 100px; height: 100px; border - radius: 50 % " src="data: image / jpeg; base64, ' + response.Data.DashboardSPecificResultData[i].ThumbnailImage + '" onclick="ModalboxImage(this)" />';
                                }

                                //Verify Button
                                if (response.Data.DashboardSPecificResultData[i].ResulstReason != "" || response.Data.DashboardSPecificResultData[i].ResulstReason != "") {
                                    verify_button_html = '<div class="buttons"><div class="form-group" ><button type="submit" name="SubmitBut" onclick="CallVerify(&apos;' + response.Data.DashboardSPecificResultData[i].ObjectId + '&apos;, &apos;' + response.Data.DashboardSPecificResultData[i].CRN + '&apos;,&apos;' + get_event +'&apos;)" class="SubmitBtn btn btn-info text-dark" data-loading-text="<i class=fa fa-spinner fa-spin></i> Loading..">VERIFY</button></div></div>';
                                }

                                //Gate Failed At
                                var result_reason_array = ["Invalid Face", "Sun Glass", "Invalid Angle", "Face not detected", "Face mismatch"];
                                if (jQuery.inArray(response.Data.DashboardSPecificResultData[i].ResulstReason, result_reason_array) != -1) {
                                    failure_at = "Gate 1";
                                }
                                else if (response.Data.DashboardSPecificResultData[i].ResulstReason == "Fake") {
                                    failure_at = "Gate 2";
                                }
                                else if (response.Data.DashboardSPecificResultData[i].ResulstReason == "Not Raise Hand" || response.Data.DashboardSPecificResultData[i].ResulstReason == "Not Smile") {
                                    failure_at = "Gate 3";
                                }
                                else if (response.Data.DashboardSPecificResultData[i].ResulstReason == "Azure Registration Failed" || response.Data.DashboardSPecificResultData[i].ResulstReason == "Azure Verification Failed") {
                                    failure_at = "Azure";
                                }
                                if (response.Data.DashboardSPecificResultData[i].ResulstReason != "" || response.Data.DashboardSPecificResultData[i].ResulstReason != null) {
                                    result_reason = response.Data.DashboardSPecificResultData[i].ResulstReason;
                                }

                                $('#success_datatable tbody').append('<tr><td>' + response.Data.DashboardSPecificResultData[i].CRN + '</td> <td>' + response.Data.DashboardSPecificResultData[i].CreatedOn + '</td> <td>' + result_reason + '</td> <td>' + response.Data.DashboardSPecificResultData[i].Version + '</td> <td>' + verify_button_html + '</td> <td>' + thumbnail_image_html + ' </td> <td>' + failure_at + ' </td> <td>' + response.Data.DashboardSPecificResultData[i].DeviceDetails + '</td></tr>');
                            }

                        }
                        
                  
                        if (Number(JSON.parse(send_data).record_limit) < response.Data.total_records) {
                            $("#top_div").show();
                            $("#bottom_div").show();
                            //$('#top_starting_index').html(Number(JSON.parse(send_data).offset_value) + 1);
                            //$('#bottom_starting_index').html(Number(JSON.parse(send_data).offset_value) + 1);

                            if ((Number(JSON.parse(send_data).offset_value) + Number(JSON.parse(send_data).record_limit)) >= response.Data.total_records) {
                                $('#top_ending_index').html(response.Data.total_records);
                                $('#bottom_ending_index').html(response.Data.total_records);
                                $('.next_btn').attr("disabled", true);
                                $('.last_btn').attr("disabled", true);
                                $('.previous_btn').attr("disabled", false);
                                $('.first_btn').attr("disabled", false);
                            }
                            else {
                                if (Number(JSON.parse(send_data).offset_value) == 0 || Number(JSON.parse(send_data).offset_value) == 1) {
                                    $('.next_btn').attr("disabled", false);
                                    $('.last_btn').attr("disabled", false);
                                    $('.previous_btn').attr("disabled", true);
                                    $('.first_btn').attr("disabled", true);
                                }
                                else {
                                    $('.next_btn').attr("disabled", false);
                                    $('.last_btn').attr("disabled", false);
                                    $('.previous_btn').attr("disabled", false);
                                    $('.first_btn').attr("disabled", false);
                                }
                                //$('#top_ending_index').html(Number(JSON.parse(send_data).offset_value) + Number(JSON.parse(send_data).record_limit));
                                //$('#bottom_ending_index').html(Number(JSON.parse(send_data).offset_value) + Number(JSON.parse(send_data).record_limit));
                            }
                            $('#top_total_count').html(response.Data.total_records);
                            $('#bottom_total_count').html(response.Data.total_records);

                        }
                        else {
                            $("#top_div").hide();
                            $("#bottom_div").hide();
                        }
                        $('#success_datatable').show();
                        $('#no_data_div').hide();
                        $('#top_div').show();
                        $('#bottom_div').show();
                    }
                }
                else {
                    $('#success_datatable').hide();
                    $('#no_data_div').show();
                    $('#top_div').hide();
                    $('#bottom_div').hide();
                }
            }
            catch (err) {
                console.log(err);
                $('#success_datatable').hide();
                $('#no_data_div').show();
            }
        },
        error: function (response) {
            $('#success_datatable').hide();
            $('#no_data_div').show();
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


//Pagination
$('.first_btn').click(function (e) {
    e.preventDefault();
    var record_limit = 10;
    $('#top_starting_index').html("1");
    $('#bottom_starting_index').html("1");
    $('#top_ending_index').html(Number(record_limit));
    $('#bottom_ending_index').html(Number(record_limit));
    var offset_value = $('#top_starting_index').html();
    page_initialize(Number(offset_value)-1, record_limit);
});

$('.previous_btn').click(function (e) {
    e.preventDefault();
    var record_limit = 10;
    var top_ending_index = $('#top_ending_index').html();
    var top_total_count = $('#top_total_count').html();
    var last_start_index = (Number(top_total_count) % Number(record_limit));
    if (top_ending_index == top_total_count) {
        $('#top_ending_index').html(Number($('#top_ending_index').html()) - last_start_index);
        $('#bottom_ending_index').html(Number($('#bottom_ending_index').html()) - last_start_index);
        $('#top_starting_index').html(Number($('#top_starting_index').html()) - Number(record_limit));
        $('#bottom_starting_index').html(Number($('#bottom_starting_index').html()) - Number(record_limit));
    }
    else {
        $('#top_ending_index').html(Number($('#top_ending_index').html()) - Number(record_limit));
        $('#bottom_ending_index').html(Number($('#bottom_ending_index').html()) - Number(record_limit));
        $('#top_starting_index').html(Number($('#top_starting_index').html()) - Number(record_limit));
        $('#bottom_starting_index').html(Number($('#bottom_starting_index').html()) - Number(record_limit));
    }
    var offset_value = $('#top_starting_index').html();
    page_initialize(Number(offset_value)-1, record_limit);
});

$('.next_btn').click(function (e) {
    e.preventDefault();
    var record_limit = 10;
    $('#top_starting_index').html(Number($('#top_starting_index').html()) + Number(record_limit));
    $('#bottom_starting_index').html(Number($('#bottom_starting_index').html()) + Number(record_limit));
    $('#top_ending_index').html(Number($('#top_ending_index').html()) + Number(record_limit));
    $('#bottom_ending_index').html(Number($('#bottom_ending_index').html()) + Number(record_limit));
    var offset_value = $('#top_starting_index').html();
    page_initialize(Number(offset_value) - 1, record_limit);
});

$('.last_btn').click(function (e) {
    e.preventDefault();
    var record_limit = 10;
    var top_total_count = Number($('#top_total_count').html());
    var last_start_index = Number(top_total_count) - (Number(top_total_count) % Number(record_limit));
    if (Number(top_total_count) == Number(last_start_index)) {
        $('#top_starting_index').html(Number(last_start_index) - Number(record_limit)+1);
        $('#bottom_starting_index').html(Number(last_start_index) - Number(record_limit)+1);
    }
    else {
        $('#top_starting_index').html(Number(last_start_index)+1);
        $('#bottom_starting_index').html(Number(last_start_index)+1);
    }

    var offset_value = $('#top_starting_index').html();
    page_initialize(offset_value,record_limit);
});

function CallVerify(ObjectId, CRN, Event) {
    //console.log(event);
    let but = event.currentTarget;
    $(".SubmitBtn").prop('disabled', true);
    but.innerHTML = but.getAttribute("data-loading-text");
    let send_data = JSON.stringify({ ObjectId: ObjectId, CRN: CRN, Event: Event });
    $.ajax({
        type: "POST",
        contentType: "application/json",
        datatype: "json",
        data: send_data,
        url: "/Home/DoVerification",
        success: function (response) {
            //let json = JSON.parse(response);
            //console.log(response);
            if (response.StatusCode == 200) {
                Modalbox(response.Message, response.Image);
            }
            else {
                ErrorModalbox(response.Message);
            }

            //console.log(response.StatusCode);
            but.innerHTML = "VERIFY";
            $(".SubmitBtn").prop('disabled', false);
        },
        error: function (response) {
            ErrorModalbox("Something went wrong");
            but.innerHTML = "VERIFY";
            $(".SubmitBtn").prop('disabled', false);
        }
    });
}
