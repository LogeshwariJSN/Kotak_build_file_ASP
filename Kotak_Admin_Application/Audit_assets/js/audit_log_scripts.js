$(document).ready(function () {
    //Initialization
    //var set_value = 0;
    //var send_ajax = 0;
    $('.even').hide();
    var compare_date1 = "";
    var compare_date2 = "";
    var table = "";
    //var current_date = new Date();
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    today = yyyy + '-' + mm + '-' + dd;
    $('#from_date_filter').val(today);
    $('#to_date_filter').val(today);

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

    $("#search_btn").click(function (e) {
        e.preventDefault();
        //get_audit_log_page(crn, event_id, result_reason, gate_number, from_date, to_date, version, offset_value, record_limit);
        var offset_value = 0;
        var record_limit = 10;
        page_initialize(offset_value, record_limit, 0);

    });


    $("#download_btn").click(function (e) {
        e.preventDefault();
        audit_log_excel_download();
        user_image_zip_download();
    });

    //Reset Button Function
    $('#reset_btn').on('click', function () {
        window.location.href = '/Home/Index/';
    });


    //Pagination
    $('.first_btn').click(function (e) {
        e.preventDefault();
        var record_limit = 10;
        $('#top_starting_index').html("1");
        $('#bottom_starting_index').html("1");
        $('#top_ending_index').html(Number(record_limit));
        $('#bottom_ending_index').html(Number(record_limit));
        var offset_value = $('#top_starting_index').html();
        page_initialize(Number(offset_value) - 1, record_limit, 1);
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
        page_initialize(Number(offset_value) - 1, record_limit, 2);
    });

    $('.next_btn').click(function (e) {
        e.preventDefault();
        var record_limit = 10;
        var top_ending_index = Number($('#top_ending_index').html()) + Number(record_limit);
        var bottom_ending_index = Number($('#bottom_ending_index').html()) + Number(record_limit);
        var top_total_count = Number($('#top_total_count').html());
        $('#top_starting_index').html(Number($('#top_starting_index').html()) + Number(record_limit));
        $('#bottom_starting_index').html(Number($('#bottom_starting_index').html()) + Number(record_limit));
        if (top_ending_index >= top_total_count) {
            $('#top_ending_index').html(top_total_count);
            $('#bottom_ending_index').html(top_total_count);
            $('.next_btn').attr("disabled", true);
            $('.last_btn').attr("disabled", true);
        }
        else {

            $('#top_ending_index').html(Number($('#top_ending_index').html()) + Number(record_limit));
            $('#bottom_ending_index').html(Number($('#bottom_ending_index').html()) + Number(record_limit));
        }

        var offset_value = $('#top_starting_index').html();
        page_initialize(Number(offset_value) - 1, record_limit, 3);
    });

    $('.last_btn').click(function (e) {
        e.preventDefault();
        var record_limit = 10;
        var top_total_count = Number($('#top_total_count').html());
        var last_start_index = Number(top_total_count) - (Number(top_total_count) % Number(record_limit));
        if (Number(top_total_count) == Number(last_start_index)) {
            $('#top_starting_index').html(Number(last_start_index) - Number(record_limit) + 1);
            $('#bottom_starting_index').html(Number(last_start_index) - Number(record_limit) + 1);
        }
        else {
            $('#top_starting_index').html(Number(last_start_index) + 1);
            $('#bottom_starting_index').html(Number(last_start_index) + 1);
        }

        var offset_value = $('#top_starting_index').html();
        page_initialize(Number(offset_value) - 1, record_limit, 4);
    });


    //Pagination
    var offset_value = 0;
    var record_limit = 10;
    var button_id = 0;
    page_initialize(offset_value, record_limit, button_id);

});

//Getting input values
function page_initialize(offset_value, record_limit, button_id) {

    var crn = $('#crn_filter').val();
    var event_id = $('#events_filter').val();
    var result_reason = $('#result_reason_filter').val();
    var gate_number = $('#gate_filter').val();
    var from_date = $('#from_date_filter').val();
    var to_date = $('#to_date_filter').val();
    var version = $('#version_filter').val();

    //if (crn == "" || event_id == "" || result_reason == "" || gate_number == "" || version =="") {
    //    window.location = "";
    //}
    //else {
    get_audit_log_page(crn, event_id, result_reason, gate_number, from_date, to_date, version, offset_value, record_limit, button_id);
    audit_log_register_attempt();
    
    // }
}

function get_audit_log_page(crn, event_id, result_reason, gate_number, from_date, to_date, version, offset_value, record_limit, button_id) {
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
                        $('#audit_log_table').hide();
                        $('#no_data_div').show();
                        $("#top_div").hide();
                        $("#bottom_div").hide();
                        $('#audit_log_table tbody').empty();
                        $('#audit_log_table thead tr').empty();
                    }
                    else {
                        $('#audit_log_table tbody').empty();
                        $('#audit_log_table thead tr').empty();
                        $('#top_total_count').html(response.total_records);
                        $('#bottom_total_count').html(response.total_records);
                        
                        //Audit Log Data
                        $("#audit_log_table thead tr").append("<th></th><th>S No<th>CRN</th><th>Date and Time</th><th>Event</th><th>Version</th><th>Transaction Status</th><th>Gate Failure</th><th>Result Reason</th><th>Channel</th>");
                        $("#audit_log_table").dataTable().fnDestroy();
                        var json_arr = [];
                        //debugger;
                        var serial_number = $("#top_starting_index").html();
                        for (var i = 0; i < response.Data.length; i++) {
                            var json_obj = { "SNo": Number(serial_number),"CRN": response.Data[i].CRN, "CreatedOn": response.Data[i].CreatedOn, "EventName": response.Data[i].EventName, "Version": response.Data[i].Version, "StatusName": response.Data[i].StatusName, "GateFailedAt": response.Data[i].GateFailedAt, "ResultReason": response.Data[i].ResultReason, "ChannelName": response.Data[i].ChannelName, "AuditLogExpandFetch": response.Data[i].AuditLogExpandFetch };
                                    json_arr.push(json_obj);
                           
                           // $('#audit_log_table tbody').append('<tr><td></td><td>' + response.Data[i].CRN + '</td> <td>' + response.Data[i].CreatedOn + '</td> <td>' + response.Data[i].EventName + '</td> <td>' + response.Data[i].Version + '</td> <td>' + response.Data[i].StatusName + '</td> <td>' + response.Data[i].GateFailedAt + ' </td> <td>' + response.Data[i].ResultReason + '</td>  <td>' + response.Data[i].ChannelName + '</td></tr>');
                            serial_number++;
                        }

                     
                        

                        if (Number(JSON.parse(send_data).record_limit) < Number(response.total_records)) {
                            //debugger;
                            $("#top_div").show();
                            $("#bottom_div").show();


                            if (button_id == 4) {
                                $('#top_starting_index').html(Number(JSON.parse(send_data).offset_value) + 1);
                                $('#bottom_starting_index').html(Number(JSON.parse(send_data).offset_value) + 1);
                                $('#top_ending_index').html($('#top_total_count').html());
                                $('#bottom_ending_index').html($('#bottom_total_count').html());
                            }
                            else {
                                $('#top_starting_index').html(Number(JSON.parse(send_data).offset_value) + 1);
                                $('#bottom_starting_index').html(Number(JSON.parse(send_data).offset_value) + 1);
                            }

                            if ((Number(JSON.parse(send_data).offset_value) + Number(JSON.parse(send_data).record_limit)) >= response.Data.total_records) {
                                $('#top_ending_index').html(response.Data.total_records);
                                $('#bottom_ending_index').html(response.Data.total_records);
                                $('.next_btn').attr("disabled", true);
                                $('.last_btn').attr("disabled", true);
                                $('.previous_btn').attr("disabled", false);
                                $('.first_btn').attr("disabled", false);
                            }
                            else {
                                if (Number(JSON.parse(send_data).offset_value) == 0) {
                                    $('.next_btn').attr("disabled", false);
                                    $('.last_btn').attr("disabled", false);
                                    $('.previous_btn').attr("disabled", true);
                                    $('.first_btn').attr("disabled", true);
                                }
                                else {
                                    if (button_id == 4) {
                                        $('.next_btn').attr("disabled", true);
                                        $('.last_btn').attr("disabled", true);
                                    }
                                    $('.previous_btn').attr("disabled", false);
                                    $('.first_btn').attr("disabled", false);
                                }
                                //$('#top_ending_index').html(Number(JSON.parse(send_data).offset_value) + Number(JSON.parse(send_data).record_limit));
                                //$('#bottom_ending_index').html(Number(JSON.parse(send_data).offset_value) + Number(JSON.parse(send_data).record_limit));
                            }
                            $('#top_total_count').html(response.total_records);
                            $('#bottom_total_count').html(response.total_records);

                        }
                        else {
                            $("#top_div").hide();
                            $("#bottom_div").hide();
                        }
                        //$('#audit_log_table').dataTable({ searching: false, paging: false, info: false });
                        table = $('#audit_log_table').DataTable({
                           
                            "data": json_arr,
                            'columns': [
                                {
                                    'className': 'details-control',
                                    'orderable': false,
                                    'data': null,
                                    'defaultContent': ''
                                },
                                { 'data': 'SNo' },
                                { 'data': 'CRN' },
                                { 'data': 'CreatedOn' },
                                { 'data': 'EventName' },
                                { 'data': 'Version' },
                                { 'data': 'StatusName' },
                                { 'data': 'GateFailedAt' },
                                { 'data': 'ResultReason' },
                                { 'data': 'ChannelName' }
                                //{ 'data': 'AuditLogExpandFetch' }
                                
                            ],
                            "bPaginate": false,
                            "aaSorting": [],
                            "info": false,
                            "searching":false
                        });

                        $('#audit_log_table').show();
                        $('#no_data_div').hide();

                    }
                }
                else {
                    $('#audit_log_table').hide();
                    $('#no_data_div').show();
                    $('#top_div').hide();
                    $('#bottom_div').hide();
                }
            }
            catch (err) {
                console.log(err);
                $('#audit_log_table').hide();
                $('#no_data_div').show();
            }
        },
        error: function (response) {
            $('#audit_log_table').hide();
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
    var crn = $('#crn_filter').val().trim();
    var event_id = $('#events_filter').val();
    var result_reason = $('#result_reason_filter').val();
    var gate_number = $('#gate_filter').val();
    var from_date = $('#from_date_filter').val();
    var to_date = $('#to_date_filter').val();
    var version = $('#version_filter').val();

    var main_obj = { crn: crn, event_id: event_id, result_reason: result_reason, gate_number: gate_number, from_date: from_date, to_date: to_date, version: version }
    var send_data = JSON.stringify(main_obj);

    console.log(send_data);

    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/AuditLogRegUsers/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {

            $('#RegCompleteSingleAttempt').html(response.RegData.reg_complete_single_attempt);
            $('#RegCompleteMultipleAttempt').html(response.RegData.reg_complete_multiple_attempt);
            $('#RegAverageCompleteUsers').html(response.RegData.reg_average_completed_users);
            $('#RegDropSingleAttempt').html(response.RegData.reg_drop_single_attempt);
            $('#RegDropMultipleAttempt').html(response.RegData.reg_drop_multiple_attempt);
            $('#RegAverageDroppedUsers').html(response.RegData.reg_average_dropped_users);
            $('#RegSuccessfulAttempts').html(response.RegData.reg_successful_attempts);
            $('#RegFailureAttempts').html(response.RegData.reg_failed_attempts);


            $('#VerifyCompleteSingleAttempt').html(response.VerifyData.verify_complete_single_attempt);
            $('#VerifyCompleteMultipleAttempt').html(response.VerifyData.verify_complete_multiple_attempt);
            $('#VerifyAverageCompleteUsers').html(response.VerifyData.verify_average_completed_users);
            $('#VerifyDropSingleAttempt').html(response.VerifyData.verify_drop_single_attempt);
            $('#VerifyDropMultipleAttempt').html(response.VerifyData.verify_drop_multiple_attempt);
            $('#VerifyAverageDroppedUsers').html(response.VerifyData.verify_average_dropped_users);
            $('#VerifySuccessfulAttempts').html(response.VerifyData.verify_successful_attempts);
            $('#VerifyFailureAttempts').html(response.VerifyData.verify_failed_attempts);

            $("#RegVerifyDone").html(Number(response.VerifyData.verify_successful_attempts) + Number(response.VerifyData.verify_failed_attempts));

        },
        error: function (response) {

        }

    });


}

//Audit Log Excel Download details
function audit_log_excel_download(){
    var crn = $('#crn_filter').val().trim();
    var event_id = $('#events_filter').val();
    var result_reason = $('#result_reason_filter').val();
    var gate_number = $('#gate_filter').val();
    var from_date = $('#from_date_filter').val();
    var to_date = $('#to_date_filter').val();
    var version = $('#version_filter').val();

    var main_obj = { crn: crn, event_id: event_id, result_reason: result_reason, gate_number: gate_number, from_date: from_date, to_date: to_date, version: version }
    var send_data = JSON.stringify(main_obj);

    console.log(send_data);

    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/get_excel_details_download/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {
            console.log(response);
          

                var headers = {
                    crn_number: "crn_number",
                    created_on: "created_on",
                    event: "event",
                    version: "version",
                    status: "status",
                    failed_at: "failed_at",
                    result_reason: "result_reason",
                    image_details: "image_details",
                    gate: "gate",
                    //json_response:"json_response",
                    confidence_score: "confidence_score",
                    threshold_value: "threshold_value",
                };

                var excel_array = [];
            for (var i = 0; i < response.ExcelDownloadData.length; i++) {
                var excel_download_details = { created_on: response.ExcelDownloadData[i].CreatedOn, crn_number: response.ExcelDownloadData[i].CRN, event: response.ExcelDownloadData[i].EventName, version: response.ExcelDownloadData[i].Version, status: response.ExcelDownloadData[i].StatusName, failed_at: response.ExcelDownloadData[i].GateFailedAt, result_reason: response.ExcelDownloadData[i].ResultReason, image_details: response.ExcelDownloadData[i].CRN + "_" + response.ExcelDownloadData[i].CreatedOn, gate: response.ExcelDownloadData[i].GATE_NAME, confidence_score: get_confidence_score_excel(response.ExcelDownloadData[i].json_response), threshold_value: response.ExcelDownloadData[i].threshold_value };
                    excel_array.push(excel_download_details);
                }

                var fileTitle = 'Audit Log Details';
                exportExcelFile(headers, excel_array, fileTitle);
            
        },
        error: function (response) {

        }

    });

}


function get_confidence_score_expand(json_response) {
    console.log(json_response);
    var gate_tags_html = "";
    if (json_response != "Azure Registration Succeeded" && json_response != "Azure Registration Failed" && json_response != "Azure Verification Succeeded" && json_response != "Azure Verification Failed" && json_response != "Azure De-Registration Succeeded") {

        var response_json = JSON.parse(json_response);

        if (response_json.Faces != undefined)
            gate_tags_html += '<p>' + response_json.Faces + '</p>'

        if (response_json.KotakComputerVisionGate1 != undefined) {
            for (var j = 0; j < response_json.KotakComputerVisionGate1.ResponseFromGate.Tags.length; j++) {
                gate_tags_html += '<p><b>' + response_json.KotakComputerVisionGate1.ResponseFromGate.Tags[j].TagName + ':</b> ' + response_json.KotakComputerVisionGate1.ResponseFromGate.Tags[j].Confidence + '</p>'
            }
        }
        if (response_json.KotakComputerVisionGate2 != undefined) {
            for (var j = 0; j < response_json.KotakComputerVisionGate2.ResponseFromGate.Tags.length; j++) {
                gate_tags_html += '<p><b>' + response_json.KotakComputerVisionGate2.ResponseFromGate.Tags[j].TagName + ':</b> ' + response_json.KotakComputerVisionGate1.ResponseFromGate.Tags[j].Confidence + '</p>'
            }
        }
    }

    return gate_tags_html;
}

function get_confidence_score_excel(json_response) {
    console.log(json_response);
    var gate_tags_html = "";
    if (json_response != "Azure Registration Succeeded" && json_response != "Azure Registration Failed" && json_response != "Azure Verification Succeeded" && json_response != "Azure Verification Failed" && json_response != "Azure De-Registration Succeeded") {

        var response_json = JSON.parse(json_response);

        if (response_json.Faces != undefined)
            gate_tags_html += + response_json.Faces + ','

        if (response_json.KotakComputerVisionGate1 != undefined) {
            for (var j = 0; j < response_json.KotakComputerVisionGate1.ResponseFromGate.Tags.length; j++) {
                gate_tags_html +=  response_json.KotakComputerVisionGate1.ResponseFromGate.Tags[j].TagName + ':' + response_json.KotakComputerVisionGate1.ResponseFromGate.Tags[j].Confidence + ','
            }
        }
        if (response_json.KotakComputerVisionGate2 != undefined) {
            for (var j = 0; j < response_json.KotakComputerVisionGate2.ResponseFromGate.Tags.length; j++) {
                gate_tags_html +=  response_json.KotakComputerVisionGate2.ResponseFromGate.Tags[j].TagName + ':' + response_json.KotakComputerVisionGate1.ResponseFromGate.Tags[j].Confidence + ','
            }
        }
    }

    return gate_tags_html;
}

// Add event listener for opening and closing details
$('#audit_log_table tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = table.row(tr);

    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    } else {
        // Open this row
        row.child(format(row.data())).show();
        tr.addClass('shown');
    }
});

// Handle click on "Expand All" button
$('#btn-show-all-children').on('click', function () {
    // Enumerate all rows
    table.rows().every(function () {
        // If row has details collapsed
        if (!this.child.isShown()) {
            // Open this row
            this.child(format(this.data())).show();
            $(this.node()).addClass('shown');
        }
    });
});

// Handle click on "Collapse All" button
$('#btn-hide-all-children').on('click', function () {
    // Enumerate all rows
    table.rows().every(function () {
        // If row has details expanded
        if (this.child.isShown()) {
            // Collapse row details
            this.child.hide();
            $(this.node()).removeClass('shown');
        }
    });
});

function format(d) {
    var tr_html = "";
    for (var i = 0; i < d.AuditLogExpandFetch.length; i++) {
        //debugger;
        tr_html += '<tr>' +
            '<td><img  style="width:100px; height:100px; border-radius:50%" src =' + d.AuditLogExpandFetch[i].Image_get + '></td>' +
            '<td>' + d.AuditLogExpandFetch[i].GATE_NAME + '</td>' +
            '<td>' + get_confidence_score_expand(d.AuditLogExpandFetch[i].JSON_RESPONSE) + '</td>' +
            '<td>' + d.AuditLogExpandFetch[i].threshold_value + '</td>' +
            '<td><a onclick="modalJsonResponse(' + d.AuditLogExpandFetch[i].AUDIT_ID +')" style="cursor:pointer;text-decoration:underline;">JSON Response</a><p style="display:none;" id="json_response' + d.AuditLogExpandFetch[i].AUDIT_ID +'">' + d.AuditLogExpandFetch[i].JSON_RESPONSE +'<p></td>' +
            '</tr>';
    }
    return ' <table class="table table-borderless">' +
        '<th> User Image</th>' +
        '<th> Gate #</th>' +
        '<th> Confidence Score</th>' +
        '<th> Threshold Value</th>' +
        '<th> JSON</th>' + tr_html
        '</table>';
}

function modalJsonResponse(audit_id) {
    var json_response = $('#json_response' + audit_id).html();
    let html = '<div class="modal show" id="alert_popup" role="dialog" data-keyboard="false" data-backdrop="static"><div class="modal-dialog"><div class="modal-content"><div style="display:table-cell; vertical-align:middle;" class="modal-body"><pre>' + json_response + '</pre><br><br><center><button type="button" id="modalButton" class="btn btn-primary"> Close </button></center></div></div></div></div>';

    document.getElementById("alert_popup_div").innerHTML = html;
    $('#alert_popup').modal('show');
    $("#modalButton").click(function () {
        $('#alert_popup').modal('hide');
    });
}


function exportExcelFile(headers, items, fileTitle) {
    if (headers) {
        items.unshift(headers);
    }

    // Convert Object to JSON
    var jsonObject = JSON.stringify(items);
    var excel = this.convertToExcel(jsonObject);
    var exportedFilename = fileTitle + '.csv' || 'export.csv';
    var blob = new Blob([excel], { type: 'text/csv;charset=utf-8;' });

    if (navigator.msSaveBlob) { // IE 10+
        navigator.msSaveBlob(blob, exportedFilename);
    } else {
        var link = document.createElement("a");
        if (link.download !== undefined) { // feature detection
            // Browsers that support HTML5 download attribute
            var url = URL.createObjectURL(blob);
            link.setAttribute("href", url);
            link.setAttribute("download", exportedFilename);
            link.style.visibility = 'hidden';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
}

function convertToExcel(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';

    for (var i = 0; i < array.length; i++) {
        var line = '';
        for (var index in array[i]) {
            if (line != '') line += ','
            line += array[i][index];
        }
        str += line + '\r\n';
    }
    return str;
}


//Audit Log User Image Zip Download details
function user_image_zip_download() {
    var crn = $('#crn_filter').val().trim();
    var event_id = $('#events_filter').val();
    var result_reason = $('#result_reason_filter').val();
    var gate_number = $('#gate_filter').val();
    var from_date = $('#from_date_filter').val();
    var to_date = $('#to_date_filter').val();
    var version = $('#version_filter').val();

    var main_obj = { crn: crn, event_id: event_id, result_reason: result_reason, gate_number: gate_number, from_date: from_date, to_date: to_date, version: version }
    var send_data = JSON.stringify(main_obj);

    console.log(send_data);

    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/get_user_images_zip_download/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {
            console.log(response);

        },
        error: function (response) {

        }

    });

}

