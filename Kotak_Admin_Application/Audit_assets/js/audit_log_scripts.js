$(document).ready(function () {
    //Initialization
    //var set_value = 0;
    //var send_ajax = 0;
    $('.even').hide();
    var compare_date1 = "";
    var compare_date2 = "";
    //var current_date = new Date();
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    today = yyyy + '-' + mm + '-' + dd;

    audit_log_register_attempt();
    audit_log_verify_attempt();


    //Clear Audit Log Click Function
    $('#clear_audit_log').on('click', function () {

        var data = JSON.stringify({
            data: "clear"
        });

        $.ajax({
            type: "POST",
            contentType: "application/json",
            datatype: "json",
            data: data,
            url: "/Home/TruncateAuditLog/",
            success: function (data) {
                document.location.reload(true);
            }
        });

    });


    //Reset Button Function
    $('#reset_btn').on('click', function () {
        window.location.href = '/Home/Index/';
    });

    //Pagination
    var offset_value = 0;
    var record_limit = 10;
    page_initialize(offset_value, record_limit);

});

//Getting input values
function page_initialize(offset_value, record_limit) {

    var crn = $('#crn_filter').val(); 
    var event_id = $('#events_filter').val();
    var result_reason= $('#result_reason_filter').val();
    var gate_number = $('#gate_filter').val();
    var from_date = $('#from_date_filter').val();
    var to_date = $('#to_date_filter').val();
    var version = $('#version_filter').val();
     
    //if (crn == "" || event_id == "" || result_reason == "" || gate_number == "" || version =="") {
    //    window.location = "";
    //}
    //else {
        get_audit_log_page(crn, event_id, result_reason, gate_number, from_date, to_date, version, offset_value, record_limit);
   // }
}

function get_audit_log_page(crn, event_id, result_reason, gate_number, from_date, to_date, version, offset_value, record_limit) {
    var main_obj = { crn: crn, event_id: event_id, result_reason: result_reason, gate_number: gate_number, from_date: from_date, to_date: to_date, version: version, offset_value: offset_value, record_limit: record_limit }
    var send_data = JSON.stringify(main_obj);

    console.log(send_data);
    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/get_audit_log_page/",
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
                                var thumbnail_image_html = "", verify_button_html = "", failure_at = "", result_reason = "";
                                var get_event = JSON.parse(send_data).event_id == "1" ? "Registration" : "Verification";
                                if (response.Data.DashboardSPecificResultData[i].ThumbnailImage != "" || response.Data.DashboardSPecificResultData[i].ThumbnailImage != null) {
                                    thumbnail_image_html = '<img style="width: 100px; height: 100px; border - radius: 50 % " src="data: image / jpeg; base64, ' + response.Data.DashboardSPecificResultData[i].ThumbnailImage + '" onclick="ModalboxImage(this)" />';
                                }

                                //Verify Button
                                if (response.Data.DashboardSPecificResultData[i].ResulstReason != "" || response.Data.DashboardSPecificResultData[i].ResulstReason != "") {
                                    verify_button_html = '<div class="buttons"><div class="form-group" ><button type="submit" name="SubmitBut" onclick="CallVerify(&apos;' + response.Data.DashboardSPecificResultData[i].ObjectId + '&apos;, &apos;' + response.Data.DashboardSPecificResultData[i].CRN + '&apos;,&apos;' + get_event + '&apos;)" class="SubmitBtn btn btn-info text-dark" data-loading-text="<i class=fa fa-spinner fa-spin></i> Loading..">VERIFY</button></div></div>';
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



function success_popup(message) {
    var html = '<div class="modal fade" id="alert_popup" role="dialog"><div class="modal-dialog"><div class="modal-content">        <div class="modal-body">          <center><h4>"Something went wrong Please try again"</h4><br><a href="/Home/index/" type="button" class="btn btn-default" style="background:#013567;">Okay</a>           </center>        </div>      </div>    </div>  </div>';
    document.getElementById("alert_popup_div").innerHTML = html;
    $('#alert_popup').modal('show');
}

//Audit Log Register Count
function audit_log_register_attempt() {
    var crn = $('#crn_filter').val();
    var event_id = $('#events_filter').val();
    var result_reason = $('#result_reason_filter').val();
    var gate_number = $('#gate_filter').val();
    var from_date = $('#from_date_filter').val();
    var to_date = $('#to_date_filter').val();
    var version = $('#version_filter').val();
     
    var main_obj = { crn: crn, event_id: 1, result_reason: result_reason, gate_number: 1, from_date: from_date, to_date: to_date, version: version}
    var send_data = JSON.stringify(main_obj);

    console.log(send_data);

    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/AuditLogRegUsers/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {

            $('#RegCompleteSingleAttempt').html(response.Data.reg_complete_single_attempt);
            $('#RegCompleteMultipleAttempt').html(response.Data.reg_complete_multiple_attempt);
            $('#RegAverageCompleteUsers').html(response.Data.reg_average_completed_users);
            $('#RegDropSingleAttempt').html(response.Data.reg_drop_single_attempt);
            $('#RegDropMultipleAttempt').html(response.Data.reg_drop_multiple_attempt);
            $('#RegAverageDroppedUsers').html(response.Data.reg_average_dropped_users);
            $('#RegSuccessfulAttempts').html(response.Data.reg_successful_attempts);
            $('#RegFailureAttempts').html(response.Data.reg_failed_attempts);

        },
        error: function (response) {

        }

});

}

//Audit Log Verify Count
function audit_log_verify_attempt() {
    var crn = $('#crn_filter').val();
    var event_id = $('#events_filter').val();
    var result_reason = $('#result_reason_filter').val();
    var gate_number = $('#gate_filter').val();
    var from_date = $('#from_date_filter').val();
    var to_date = $('#to_date_filter').val();
    var version = $('#version_filter').val();

    var main_obj = { crn: crn, event_id: 1, result_reason: result_reason, gate_number: 1, from_date: from_date, to_date: to_date, version: version }
    var send_data = JSON.stringify(main_obj);

    console.log(send_data);

    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/AuditLogVerifyUsers/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {

            $('#VerifyCompleteSingleAttempt').html(response.Data.verify_complete_single_attempt);
            $('#VerifyCompleteMultipleAttempt').html(response.Data.verify_complete_multiple_attempt);
            $('#VerifyAverageCompleteUsers').html(response.Data.verify_average_completed_users);
            $('#VerifyDropSingleAttempt').html(response.Data.verify_drop_single_attempt);
            $('#VerifyDropMultipleAttempt').html(response.Data.verify_drop_multiple_attempt);
            $('#VerifyAverageDroppedUsers').html(response.Data.verify_average_dropped_users);
            $('#VerifySuccessfulAttempts').html(response.Data.verify_successful_attempts);
            $('#VerifyFailureAttempts').html(response.Data.verify_failed_attempts);

        },
        error: function (response) {

        }

    });

}