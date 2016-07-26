$(document).ready(function() {
    function doDraggable() {
        //$('#usersList li').css({ 'z-index': 100 }).draggable({
        //    'opacity': '0.5',
        //    'revert': true,
        //    'cursor': 'pointer'
        // });
        var viewBagIsAdmin = $("#viewbag-is-user-admin").val();
        var viewBagUserPermission = $("#viewbag-user-permission").val();

        if (viewBagIsAdmin || viewBagUserPermission) {
            $('#usersList li').draggable({
                revert: "invalid",
                containment: ".wrap-for-draggable"
            });
        }
    }

    

    $(document).ajaxComplete(function() {
        doDraggable();
    });

    // Gets All users 
    function getUsers() {
        var actionUrl = $('#GetUsers').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            success: function(data) {
                $('#users').html(data);
            }
        });
    }

    //get all users in team
    function getUserTeamPos(idVal) {
        var actionUrl = $('#GetUserTeamPos').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { id: idVal },
            success: function(data) {
                $('#userTeamPos').html(data);
                $('#selectedTeam').val(idVal);
            }
        });
    }

    function addUserToTeam(idUser, idTeam, onSuccess) {
        var actionUrl = $('#AddUserToTeam').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { userId: idUser, teamId: idTeam },
            dataType: 'json',
            success: function(data) {
                if (data.Status === true) {
                    getUserTeamPos(data.InsertedId);
                    onSuccess();
                }
            }
        });
    }

    function getTeamsByTerm(value, pageNumber) {
        var actionUrl = $('#GetTeamsByTerm').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { term: value, page: pageNumber },
            success: function(data) {
                $('#teams').html(data);
                $('#userTeamPos').empty();
            }
        });
    }

    function getUsersByTerm(value, teamId) {
        var actionUrl = $('#GetUsersByTerm').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { term: value, teamId: teamId },
            success: function(data) {
                $('#users').html(data);
            }
        });
    }

    function updateTeamsPage() {
        getTeamsByTerm($('#viewbagTeamFilter').val(), $('#viewbagTeamPage').val());
    }

    function deleteTeam(idVal) {
        var actionUrl = $('#deleteTeam').val();
        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: { id: idVal },
            success: function(data) {
                if (data.Status === true) {
                    $('#modDeleteTeamDialog').modal('toggle');
                    updateTeamsPage();
                    $('#userTeamPos').empty();
                }
            }
        });
    }

    function deleteUserTeamPos(userId, teamId) {
        var actionUrl = $('#deleteUserTeamPos').val();
        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: { userId: userId, teamId: teamId },
            success: function(data) {
                if (data.Status === true) {
                    $('#modDeleteUserTeamPosDialog').modal('toggle');
                    getUserTeamPos(teamId);
                    var value = $('#searchUserInput').val();
                    getUsersByTerm(value, teamId);
                }
            }
        });
    }

    function editUserTeamPos(userId, teamId, positionId) {
        var actionUrl = $('#EditUserTeamPosition').val();
        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: { userId: userId, teamId: teamId, positionId: positionId },
            success: function(data) {
                if (data.Status === true) {

                }
            }
        });
    }

    $(document).on('click', '.teamItem', function() {
        var teamId = $(this).children('.teamId').val();
        getUserTeamPos(teamId);
        var value = $('#searchUserInput').val();
        getUsersByTerm(value, teamId);
        $(this).addClass("selectedTeam").siblings().removeClass("selectedTeam");
        doDraggable();
        var assigTeam = $("#assignTeamBtn");
        if (assigTeam != undefined) {
            assigTeam.removeClass("hidden");
        }
    });



    $('#userTeamPos').droppable({
        drop: function(event, ui) {
            var userId = $(ui.draggable).children('.userId').val();
            var teamId = $('#selectedTeam').val();
            addUserToTeam(userId, teamId, function () {
                $(ui.draggable).remove();
            });

        }
    });
    /*Autocomplete Teams*/
    $("[data-autocomplete-source]#searchTeamInput").each(function() {
        var target = $(this);
        target.autocomplete({ source: target.attr("data-autocomplete-source") });
    });

    $('#searchTeamInput').keypress(function(e) {
        if (e.which === 13) {
            var value = $('#searchTeamInput').val();
            getTeamsByTerm(value);
            $("[data-autocomplete-source]#searchTeamInput").autocomplete('close');
            return false;
        }
    });
    /*Autocomplete Users*/
    $("[data-autocomplete-source]#searchUserInput").each(function() {
        var target = $(this);
        target.autocomplete({ source: target.attr("data-autocomplete-source") });
    });

    $('#searchUserInput').keypress(function(e) {
        if (e.which === 13) {
            var value = $('#searchUserInput').val();
            var teamId = $('.selectedTeam').attr('data-id');
            getUsersByTerm(value, teamId);

            $("[data-autocomplete-source]#searchUserInput").autocomplete('close');
            return false;
        }
    });


    $(document).on('click', '.teamEdit', function(e) {

        e.preventDefault();
        e.stopPropagation();
        $.get(this.href, function(data) {
            $('#dialogEditTeamContent').html(data);
            $('#modEditTeamDialog').modal('show');
        });
    });

    $(document).on('submit', '#EditTeamForm', function(e) {
        e.preventDefault();

        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function(data) {
                if (data.Status === true) {
                    $('#modEditTeamDialog').modal('toggle');
                    updateTeamsPage();
                } else {
                    $('#editTeamError').html(data.Message);
                }
            }
        });
    });

    $(document).on('submit', '#AddTeamForm', function(e) {
        e.preventDefault();

        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function(data) {
                if (data.Status === true) {
                    $('#modAddTeamDialog').modal('toggle');
                    getTeamsByTerm(null,1);
                } else {
                    //
                }
            }
        });
    });

    $(document).on('click', '.teamDelete', function(e) {

        e.preventDefault();
        $.get(this.href, function(data) {
            $('#dialogDeleteTeamContent').html(data);
            $('#modDeleteTeamDialog').modal('show');
        });
    });

    $(document).on('click', '#btnConfirmDeleteTeam', function() {
        var idVal = $(".teamIdForDelete").val();
        deleteTeam(idVal);
    });

    $(document).on('click', '.userTeamPosDelete', function(e) {

        e.preventDefault();
        $.get(this.href, function(data) {
            $('#dialogDeleteUserTeamPosContent').html(data);
            $('#modDeleteUserTeamPosDialog').modal('show');
        });
    });

    $(document).on('click', '#btnConfirmDeleteUserTeamPos', function() {
        var userId = $(".UserInTeamIdForDelete").val();
        var teamId = $(".TeamInUserTeamPosIdForDelete").val();

        deleteUserTeamPos(userId, teamId);
    });
     

    (function ($) {

        //re-set all client validation given a jQuery selected form or child
        $.fn.resetValidation = function () {

            var $form = this.closest('form');
            //reset jQuery Validate's internals
            $form.validate().resetForm();

            //reset unobtrusive validation summary, if it exists
            $form.find("[data-valmsg-summary=true]")
                .removeClass("validation-summary-errors")
                .addClass("validation-summary-valid")
                .find("ul").empty();

            //reset unobtrusive field level, if it exists
            $form.find("[data-valmsg-replace]")
                .removeClass("field-validation-error")
                .addClass("field-validation-valid")
                .empty();
            $('#Name').removeClass('input-validation-error');
            return $form;
        };

        //reset a form given a jQuery selected form or a child
        //by default validation is also reset
        $.fn.formReset = function (resetValidation) {
            var $form = this.closest('form');

            //$form[0].reset();

            if (resetValidation == undefined || resetValidation) {
                $form.resetValidation();
            }

            return $form;
        }
    })(jQuery);


    $("#modAddTeamDialog").on("hidden.bs.modal", function () {
        //alert("2");
        var theForm = $(this).find("#AddTeamForm");
        theForm[0].reset();
        theForm.resetValidation();
        //var theFormLog = $(this).find("#loginUser");
        //theFormLog[0].reset();
        //theFormLog.resetValidation();
        //var res = $(this).find("#result");
        //res.empty();
    });

    $(document).on('click', '#addTeamBtn', function(e) {

        $('#modAddTeamDialog').modal('show');
    });

    $(document).on('change', '.selectedPosition', function() {
        var userId = $(this).siblings('.hiddenUser').val();
        var teamId = $(this).siblings('.hiddenTeam').val();
        var positionId = $(this).val();
        editUserTeamPos(userId, teamId, positionId);
    });

    /*__________________________*/

    /*Issues*/
    function deleteIssue(idVal) {
        var actionUrl = $('#deleteIssue').val();
        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: { id: idVal },
            success: function(data) {
                if (data.Status === true) {
                    $('#modDeleteIssueDialog').modal('toggle');
                    updateIssuesPage(null, function() {});
                    $('#issue-info').empty();
                }
            }
        });
    }

    function getIssuesByTerm(value, pageNumber, projectId, sortFilter, selectedId, complete) {
        var actionUrl = $('#GetIssuesByTerm').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { term: value, page: pageNumber, projectId: $('.selected-issue-project').val(), sortFilter: sortFilter },
            success: function(data) {
                $('#issues').html(data);
                setSelectedIssue(selectedId);
                //$('#userTeamPos').empty();
                complete();
            }
        });
    }

    function updateIssuesPage(selectedId, complete) {
        getIssuesByTerm($('#viewbagIssueFilter').val(), $('#viewbagIssuePage').val(), $('#viewbagIssueProject').val(), $('#viewbagIssueSortFilter').val(), selectedId, complete);
         
        
    }

    function setSelectedIssue(selectedId) {
        if (selectedId !== undefined) {
            var element = $(".issue-item[data-id='" + selectedId + "']");
            element.addClass("selected-issue").siblings().removeClass("selected-issue");
        }
    }

    $(document).on('click', '#show-issue-menu', function () {
        $('.issue-slide-menu').toggleClass('issue-slide-menu-active');
        $('.issue-content').toggleClass('issue-content-active');

    });

    function getIssueInfo(idVal) {
        var actionUrl = $('#GetIssueInfo').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { id: idVal },
            success: function (data) {
                
                $('.issue-info').removeClass('loading-page');
                $('#issue-info').html(data);
                showEstimate(".issue-info-menu-item-estimate");
                showEstimate(".issue-info-menu-item-spent-time");
                $(".time-tracking-spent-time").each(function () {
                    showEstimate($(this));
                });

                //$('#selectedTeam').val(idVal);
            }
        });
    }

    function addParentIssue(parentId, childId) {
        var actionUrl = $('#add-parent-issue').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { parentId: parentId, childId: childId },
            success: function (data) {
                if (data.Status !== null) {
                    $('#modAddRelativeIssueDialog').modal('toggle');
                    var issueId = $('.selected-issue').children('.issue-id').val();
                    getIssueInfo(issueId);
                }
            }
        });
    }

    function deleteParentIssue(issueId,element,complete)
    {
        var actionUrl = $('#delete-parent-issue').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { issueId: issueId },
            success: function (data) {
                if (data.Status !== null) {
                    //console.log($(element).html());
                    //console.log(element);
                    //var issueId = $('.selected-issue').children('.issue-id').val();
                    //getIssueInfo(issueId);
                    element.remove();
                    complete();

                }
            }
        });
    }

    $(document).on('click', '#addIssueBtn', function (e) {
        if ($('.selected-issue-project').val() !== "") {
            $('#modAddIssueDialog').modal('show');
        }
    });
    $(document).on('click', '.issue-delete', function (e) {

        e.preventDefault();
        $.get(this.href, function (data) {
            $('#dialogDeleteIssueContent').html(data);
            $('#modDeleteIssueDialog').modal('show');
        });
    });

    $(document).on('click', '#btnConfirmDeleteIssue', function () {
        var idVal = $(".issueIdForDelete").val();
        deleteIssue(idVal);
    });

    $(document).on('submit', '#AddIssueForm', function (e) {
        e.preventDefault();

        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (data) {
                if (data.Status === true) {
                    $('#modAddIssueDialog').modal('toggle');
                    updateIssuesPage(null, clickFirstIssueChild);
                } else {
                    //
                }
            }
        });
    });
    $(document).on('click', '.issue-sort-filters li', function () {
        $("#viewbagIssueSortFilter").val($(this).attr('data-sort'));
        //$(this).attr("data-sort", "222");
        var x = $(this).attr("data-sort");
        updateIssuesPage(null, clickFirstIssueChild);
        $('#issue-info').empty();
    });
    /*Autocomplete Issues*/
    $("[data-autocomplete-source]#search-issue-input").each(function () {
        var target = $(this);
        target.autocomplete({ source: target.attr("data-autocomplete-source") });
    });

    $('#search-issue-input').keypress(function (e) {
        if (e.which === 13) {
            var value = $('#search-issue-input').val();
            getIssuesByTerm(value, null,null,null,null, clickFirstIssueChild);
            $("[data-autocomplete-source]#search-issue-input").autocomplete('close');
            return false;
        }
    });

    $(document).on('click', '.issue-item', function () {
        $('#issue-info').empty();
        $('.issue-info').addClass('loading-page');

        var issueId = $(this).children('.issue-id').val();
        $(this).addClass("selected-issue").siblings().removeClass("selected-issue");
        //$("#issue-tabs").tabs();
        getIssueInfo(issueId);
        //$(this).addClass("selectedTeam").siblings().removeClass("selectedTeam");
    });

    $(document).on('change', '.selected-issue-project', function () {
        if ($('.selected-issue-project').val() !== "") {
            $('#viewbagIssueSortFilter').val(null);
            updateIssuesPage(null, clickFirstIssueChild);
            $('#issue-info').empty();
            
        }
    });

    function clickFirstIssueChild() {
        if ($('#issue-list').children().length > 0) {
            $('.issue-item')[0].click();
        }

    }

    clickFirstIssueChild();

    $(document).on('click', '.issue-add', function (e) {

        e.preventDefault();
        e.stopPropagation();
        var projectId = $('.selected-issue-project').val();
        if (projectId !== "" && projectId !== undefined) {
            //    $.get(this.href, function(data) {
            //        $('#dialogAddIssueContent').html(data);
            //        $('#modAddIssueDialog').modal('show');
            //    });
            var actionUrl = $('#AddIssueInfo').val();
            $.ajax({
                type: 'GET',
                url: actionUrl,
                data: { id: projectId },
                success: function (data) {
                    $('#dialogAddIssueContent').html(data);
                    $('#modAddIssueDialog').modal('show');
                }
            });
        }


    });
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    $(document).on('submit', '#EditIssueForm', function (e) {
        e.preventDefault();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (data) {
                var id = $('.issue-info-description').attr('data-id');
                updateIssuesPage(id, function() {});
                
               

            }
        });
    });

    $(document).on('change', '#issue-info #EditIssueForm select', function () {
        $("#EditIssueForm").submit();
        //updateIssuesPage();
    });

    $(document).on("click", "#issue-info p.issue-name", function () {
        var issuePermission = $('#viewbag-issue-permission').val();
        if (issuePermission === "true") {
            var txt = $(".issue-name").text();
            $("#issue-info .issue-name").replaceWith("<input class='issue-name'/>");
            $("#issue-info .issue-name").val(txt);
        }
    });

    $(document).on("blur", "#issue-info input.issue-name", function () {
        var txt = $(this).val();
        $(this).replaceWith("<p class='issue-name'></p>");
        $("#issue-info .issue-name").text(txt);
        $("#issue-info #Name").val(txt);
        $("#EditIssueForm").submit();

    });

    $(document).on("click", "#issue-info pre.issue-description", function () {
        var issuePermission = $('#viewbag-issue-permission').val();
        if (issuePermission === "true") {
            var txt = $(".issue-description").text();
            $("#issue-info .issue-description").replaceWith("<textarea class='issue-description'></textarea>");
            $("#issue-info .issue-description").text(txt);
        }
    });

    $(document).on("blur", "#issue-info textarea.issue-description", function () {
        var txt = $(this).val();
        $(this).replaceWith("<pre class='issue-description'></pre>");
        $("#issue-info .issue-description").text(txt);
        $("#issue-info #Description").val(txt);
        $("#EditIssueForm").submit();
    });
    $(document).on('click', '.set-parent-issue-btn', function (e) {
        e.preventDefault();
        var actionUrl = $('#add-parent-issue-info').val();
        var projectId = $('.selected-issue-project').val();
        var issueId = $('.selected-issue').children('.issue-id').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { issueId: issueId, projectId: projectId },
            success: function (data) {
                $('#dialogRelativeIssueContent').html(data);
                $('#modAddRelativeIssueDialog').modal('show');
                $('.relative-issue-btn').addClass('save-relative-parent-btn');
                $('.relative-issue-btn').prop('disabled', true);

            }
        });
       
    });

    $(document).on('click', '.set-child-issue-btn', function (e) {
        e.preventDefault();
        var actionUrl = $('#add-child-issue-info').val();
        var projectId = $('.selected-issue-project').val();
        var issueId = $('.selected-issue').children('.issue-id').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { issueId: issueId, projectId: projectId },
            success: function (data) {
                $('#dialogRelativeIssueContent').html(data);
                $('#modAddRelativeIssueDialog').modal('show');
                $('.relative-issue-btn').addClass('save-relative-child-btn');
                $('.relative-issue-btn').prop('disabled', true);

            }
        });

    });

    $(document).on('click', '.save-relative-parent-btn', function () {
        var selectedItemId = $('.selected-relative-issue').attr('data-id');
        var issueId = $('.selected-issue').children('.issue-id').val();
        addParentIssue(selectedItemId, issueId);
        $('.relative-issue-btn').removeClass('save-relative-parent-btn');

    });

    $(document).on('click', '.save-relative-child-btn', function () {
        var selectedItemId = $('.selected-relative-issue').attr('data-id');
        var issueId = $('.selected-issue').children('.issue-id').val();
        addParentIssue(issueId, selectedItemId);
        $('.relative-issue-btn').removeClass('save-relative-child-btn');

    });

    $(document).on('click', '.issue-relative-item', function () {
        $(this).addClass("selected-relative-issue").siblings().removeClass("selected-relative-issue");
        $('.relative-issue-btn').prop('disabled', false);
    });

    $(document).on('click', '.delete-issue-child', function (e) {
        e.preventDefault();
        var id = $(this).attr('data-id');
        deleteParentIssue(id, $(this).closest('.issue-children-item'), function() {
            if ($('.list-issue-child li').length === 0) {
                $('.issue-children-header').css('display', 'none');
            }
        });
        
    });
    $(document).on('click', '.delete-issue-parent', function (e) {
        e.preventDefault();
        var id = $(this).attr('data-id');
        deleteParentIssue(id, $(this).closest('.parent-issue'), function () {
                $('.issue-parent-header').css('display', 'none');
        });
        
    });
    $("#assignTeamBtn").click(function (event) {
        event.preventDefault();
        var teamId = $("#selectedTeam").val();
        var url = $(this).attr("href");
        url = url + "&teamId=" + teamId;
        //  console.log(url);
        window.location.href = url;
    });

    var pattern = /^(\d{1,}h)?(\d{1,}m)?$/;

    //function checkEstimate(className, idForUpdate, complete) {
        
    //}

    function inputKeyPress(className, idForUpdate, complete) {
        $('.' + className).keypress(function (e) {
            if (e.which === 13) {
                if (pattern.test($(this).val())) {
                    var match = $(this).val().match(pattern);
                    var res;
                    var hours;
                    if (match[1] === undefined && match[2] === undefined) {
                        res = 0;
                    }
                    else if (match[2] != undefined) {
                        if (match[1] === undefined) {
                            hours = 0;
                        }
                        else {
                            hours = match[1].slice(0, -1);
                        }
                        var minutes = match[2].slice(0, -1);
                        res = hours * 60 + minutes * 1;
                    }
                    else {
                        hours = match[1].slice(0, -1);
                        res = hours * 60;
                    }
                    
                    $("#issue-info #" + idForUpdate).val(res);
                    $(this).toSpan(className, res);
                    $('.' + className + '-error').css({ 'display': 'none' });
                    
                    complete();

                } else {
                    //output.innerHTML = 'false';
                    $('.' + className + '-error').css({ 'display': 'block' });
                    $('input.' + className).css({ 'border': '2px solid #e74c3c' });
                }
                return false;
            }
        });
    }
    $.fn.toSpan = function (className, res) {
        var txt = this.val();
        this.replaceWith("<span class='" + className + "'></span>");
        //this.text(txt);
        $('.' + className).attr("data-issue-estimate", res);
    }

    function showEstimate(className) {
        var count = $(className).attr("data-issue-estimate");

        var hours = Math.floor(count / 60);
        var minutes = count % 60;
        var res;

        if ((hours === 0 && minutes === 0) || count === undefined) {
            res = 0;
        }
        else if (hours === 0) {
            res = minutes + 'm';
        }
        else if (minutes === 0) {
            res = hours + 'h';
        }
        else {
            res = hours + 'h' + minutes + 'm';
        }
        $(className).text(res);
    }
    $.fn.toInput = function (className, idForUpdate) {
            var txt = $("." + className).text();
            var attr = $("." + className).attr("data-issue-estimate");
            $("." + className).replaceWith("<input class='"+className+"'/>");
            $("." + className).val(txt);
            $("." + className).attr("data-issue-estimate", attr);
            inputKeyPress(className, idForUpdate, function () {
                showEstimate('.' + className);
                $("#EditIssueForm").submit();
            });
    }
    $(document).on("click", "span.issue-info-menu-item-estimate", function () {
        var issuePermission = $('#viewbag-issue-permission').val();
        if (issuePermission === "true") {
            $(this).toInput('issue-info-menu-item-estimate', 'Estimate');
        }
    });

    //$(document).on("click", "span.issue-info-menu-item-spent-time", function () {
    //$(this).toInput('issue-info-menu-item-spent-time', 'SpentTime');
    //});
    /*Time tracking*/
    function addTimeTracking(issueId, comment, estimate, timeTrackingTypeId) {
        var actionUrl = $('#add-time-tracking-action').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { issueId: issueId, comment: comment, estimate: estimate, timeTrackingTypeId: timeTrackingTypeId },
            success: function (data) {
                $('.time-tracking-list').append(data);
                $('.time-tracking-spent-time').each(function () {
                    showEstimate($(this));
                });
                $('.time-tracking-create input').val('');
                calculateSpentTime();
            }
        });
    }

    function checkEstimate(className) {
        if (pattern.test($('.' + className).val())) {
            var match = $('.' + className).val().match(pattern);
            var res;
            var hours;
            if (match[1] === undefined && match[2] === undefined) {
                res = 0;
            }
            else if (match[2] != undefined) {
                if (match[1] === undefined) {
                    hours = 0;
                }
                else {
                    hours = match[1].slice(0, -1);
                }
                var minutes = match[2].slice(0, -1);
                res = hours * 60 + minutes * 1;
            }
            else {
                hours = match[1].slice(0, -1);
                res = hours * 60;
            }
            $('.' + className + '-error').css({ 'display': 'none' });
            $('input.' + className).css({ 'border': '2px hidden transparent' });
            return res;

        } else {
            //output.innerHTML = 'false';
            $('.' + className + '-error').css({ 'display': 'block' });
            $('input.' + className).css({ 'border': '2px solid #e74c3c' });
            return false;
        }
    }
    function deleteTimeTracking(timeTrackingId, element) {
        var actionUrl = $('#delete-time-tracking').val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { timeTrackingId: timeTrackingId },
            success: function (data) {
                if (data.Status !== null) {
                    console.log(element.find('.time-tracking-spent-time').attr("data-issue-estimate"));
                    element.remove();
                    calculateSpentTime();

                }
            }
        });
    }

    function calculateSpentTime() {
        var res = 0;
        $('.time-tracking-spent-time').each(function () {
            
                res += $(this).attr("data-issue-estimate")*1;
        });
        $('.issue-info-menu-item-spent-time').attr("data-issue-estimate", res);
        showEstimate('.issue-info-menu-item-spent-time');
    }

    $(document).on('click', '.add-time-tracking-btn', function (e) {
        e.preventDefault();
        var res = checkEstimate('time-tracking-estimate-input');
        if (res !== false) {
            addTimeTracking($('.selected-issue').attr('data-id'), $('#time-tracking-comment-input').val(), res, $('#time-tracking-type-for-add').val());
        }
        
    });

    $(document).on('click', '.delete-time-tracking-item', function (e) {
        e.preventDefault();
        deleteTimeTracking($(this).attr('data-id'), $(this).closest('li'));

    });
    /*end Time tracking*/

    /*Sprint*/
    //function hiddenDelete(element) {

    //    var el = element.find('.countIssues h4').html();
    //    console.log(element.find('.countIssues h4').html());
    //    if (el > 0) {
    //        element.find('.showSprintModalDelete').addClass("hidden");
    //    } else {
    //        element.find('.showSprintModalDelete').removeClass("hidden");
    //    }
    //}
    function countIssues(sprintId, className) {
        var actionUrl = $('#CountIssues').val();
        $.ajax({
            type: "GET",
            url: actionUrl,
            data: { sprintId: sprintId },
            success: function (data) {
                //var tt = $(".ui-accordion-content-active").siblings('.sprint-line');
                className.find('.countIssues h4').html(data[0]);
                className.find('.countTaskOpen h4').html(data[1]);
                className.find('.countTaskInProgess h4').html(data[2]);
                className.find('.countTaskFixed h4').html(data[3]);
                className.find('.countTaskVerified h4').html(data[4]);
                hiddenDelete(className);
            }
        });
    }
    function showAgileBoard(sprintId, issueId) {
        var actionUrl = $("#show-agile-board-id").val();
        $.ajax({
            type: 'GET',
            url: actionUrl,
            data: { sprintId: sprintId, issueId: issueId },
            success: function (data) {
                $('.ui-accordion-content-active').html(data);
            }
        });
    }
  
    $(document).on('submit', '#AddIssueToSprintForm', function (e) {
        e.preventDefault();

        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function(data) {
                $("#modalShowAddTask").modal('hide');
                //$("body").removeClass('modal-open');
                //$(".modal-backdrop").remove();
                var element = $(".sprint-line[data-id='" + data.sprintId + "']");
                if (data.issueId !== null) {
                    showAgileBoard(data.sprintId, data.issueId);
                }
                countIssues(data.sprintId, element);
                
            }
        });
    });
});


/*avatar*/
(function ($) {
    $.fn.jInputFile = function (options) {

        return this.each(function () {

            $(this).val('');
            $(this).wrap('<div></div>');
            $(this).parent().css('height', $(this).height());
            $(this).after('<div class="jInputFile-fakeButton"></div><div class="jInputFile-blocker"></div><div class="jInputFile-activeBrowseButton jInputFile-fakeButton"></div><div class="jInputFile-fileName"></div>');
            $(this).addClass('jInputFile-customFile');

            $(this).hover(
                function () {
                    $(this).parent().children('.jInputFile-activeBrowseButton').css('display', 'block');
                },
                function () {
                    $(this).parent().children('.jInputFile-activeBrowseButton').css('display', 'none');
                }
            );
        });
    }
})(jQuery);

/* PLANNING */
function planningDragAndDrop(containerOne, containerTwo, dragItem) {
    $(dragItem).draggable(
        {
            revert: "invalid",
            containment: ".sprint-container",
            helper: "clone",
            cursor: "move",
            drag: function (event, ui) {
                $(ui.helper.prevObject).addClass("box_current");
                $('.sprint-container').addClass('highlight_container');

            },
            stop: function (event, ui) {
                $(ui.helper.prevObject).removeClass("box_current");
                $('.sprint-container').removeClass('highlight_container');
            }
            
        });
    //function hiddenDelete(element) {

    //    var el = element.find('.countIssues h4').html();
    //    console.log(element.find('.countIssues h4').html());
    //    if (el > 0) {
    //        element.find('.showSprintModalDelete').addClass("hidden");
    //    } else {
    //        element.find('.showSprintModalDelete').removeClass("hidden");
    //    }
    //}
    
    function countIssues(sprintId) {
        var actionUrl = $('#CountIssues').val();
        $.ajax({
            type: "GET",
            url: actionUrl,
            data: { sprintId: sprintId },
            success: function(data) {
                $(".sprint-panel.opened").siblings('.sprint-line').find('.countIssues h4').html(data[0]);
                $(".sprint-panel.opened").siblings('.sprint-line').find('.countTaskOpen h4').html(data[1]);
                $(".sprint-panel.opened").siblings('.sprint-line').find('.countTaskInProgess h4').html(data[2]);
                $(".sprint-panel.opened").siblings('.sprint-line').find('.countTaskFixed h4').html(data[3]);
                $(".sprint-panel.opened").siblings('.sprint-line').find('.countTaskVerified h4').html(data[4]);
                hiddenDelete($(".sprint-panel.opened").siblings('.sprint-line'));
            }
        });
    }

    function moveIssie(sprintId, issueId) {
        var actionUrl = $('#MoveIssue').val();
        $.ajax({
            type: "GET",
            url: actionUrl,
            data: { sprintId: sprintId, issueId: issueId },
            success: function () {
                var sprintId = $(".sprint-id").val();
                countIssues(sprintId);
            }
        });
    }

    function acceptBoxIngalleryOne(item) {
        $(item).fadeOut("fast", function () {
            $(containerOne).children(".issues-for-sprint").append(item);
        });

        var issueId = $(item).context.dataset.id;
        var sprintId = $(".sprint-id").val();
        moveIssie(sprintId, issueId);

        $(item).fadeIn("fast");
        $(item).removeClass("box_current");
    }

    function acceptBoxIngalleryTwo(item) {
        $(item).fadeOut("fast", function () {
            $(containerTwo).children(".issues-for-sprint").append(item);
        });

        var issueId = $(item).context.dataset.id;
        moveIssie(null, issueId);

        $(item).fadeIn("fast");
        $(item).removeClass("box_current");
    }

    $(containerTwo).droppable({
        accept: dragItem,
        hoverClass: "issue-place",
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            acceptBoxIngalleryTwo(ui.draggable);
        }
    });

    $(containerOne).droppable({
        accept: dragItem,
        hoverClass: "issue-place",
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            acceptBoxIngalleryOne(ui.draggable);
        }
    });
}

/* AgileBoard */

function agileBoardDragAndDropFull(container1, container2, container3, container4, container5, dragItem) {
    $(dragItem).draggable(
        {
            revert: "invalid",
            containment: ".board-container",
            helper: "clone",
            cursor: "move",
            drag: function (event, ui) {
                $(ui.helper.prevObject).addClass("box_current");
            },
            stop: function (event, ui) {
                $(ui.helper.prevObject).removeClass("box_current");
            }
        });

    function countIssues(sprintId) {
        var actionUrl = $('#CountIssues').val();
        $.ajax({
            type: "GET",
            url: actionUrl,
            data: { sprintId: sprintId },
            success: function (data) {
                $(".board-panel.opened").siblings('.sprint-line').find('.countIssues h4').html(data[0]);
                $(".board-panel.opened").siblings('.sprint-line').find('.countTaskOpen h4').html(data[1]);
                $(".board-panel.opened").siblings('.sprint-line').find('.countTaskInProgess h4').html(data[2]);
                $(".board-panel.opened").siblings('.sprint-line').find('.countTaskFixed h4').html(data[3]);
                $(".board-panel.opened").siblings('.sprint-line').find('.countTaskVerified h4').html(data[4]);
            }
        }); 
    }

    function moveIssie(issueId, nameState) {
        
        var actionUrl = $('#MoveIssueInBoard').val();
        $.ajax({
            type: "GET",
            url: actionUrl,
            data: { issueId: issueId, nameState: nameState },
            success: function () {
                var sprintId = $(".sprint-id").val();
                countIssues(sprintId);
            }
        });
    }

    function acceptBoxIngallery1(item) {
        $(item).fadeOut("fast", function () {
            $(container1).children(".issues-for-sprint").append(item);
        });

        var issueId = $(item).context.dataset.id;
        moveIssie(issueId, "Open");

        $(item).fadeIn("fast");
        $(item).removeClass("box_current");
        $(item).find(".number-issues").removeClass("task-ended");
    }

    function acceptBoxIngallery2(item) {
        $(item).fadeOut("fast", function () {
            $(container2).children(".issues-for-sprint").append(item);
        });

        var issueId = $(item).context.dataset.id;
        moveIssie(issueId, "Reopened");

        $(item).fadeIn("fast");
        $(item).removeClass("box_current");
        $(item).find(".number-issues").removeClass("task-ended");
    }

    function acceptBoxIngallery3(item) {
        $(item).fadeOut("fast", function () {
            $(container3).children(".issues-for-sprint").append(item);
        });

        var issueId = $(item).context.dataset.id;
        moveIssie(issueId, "In Progress");

        $(item).fadeIn("fast");
        $(item).removeClass("box_current");
        $(item).find(".number-issues").removeClass("task-ended");
    }

    function acceptBoxIngallery4(item) {
        $(item).fadeOut("fast", function () {
            $(container4).children(".issues-for-sprint").append(item);
        });
        
        var issueId = $(item).context.dataset.id;       
        moveIssie(issueId, "Fixed");

        $(item).fadeIn("fast");
        $(item).removeClass("box_current");
        $(item).find(".number-issues").addClass("task-ended");
    }

    function acceptBoxIngallery5(item) {
        $(item).fadeOut("fast", function () {
            $(container5).children(".issues-for-sprint").append(item);
        });

        var issueId = $(item).context.dataset.id;
        moveIssie(issueId, "Verified");

        $(item).fadeIn("fast");
        $(item).removeClass("box_current");
        $(item).find(".number-issues").addClass("task-ended");
    }

    $(container5).droppable({
        accept: dragItem,
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            acceptBoxIngallery5(ui.draggable);
        }
    });

    $(container4).droppable({
        accept: dragItem,
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            acceptBoxIngallery4(ui.draggable);
        }
    });

    $(container3).droppable({
        accept: dragItem,
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            acceptBoxIngallery3(ui.draggable);
        }
    });

    $(container2).droppable({
        accept: dragItem,
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            acceptBoxIngallery2(ui.draggable);
        }
    });

    $(container1).droppable({
        accept: dragItem,
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            acceptBoxIngallery1(ui.draggable);
        }
    });
}
function hiddenDelete(element) {

    var el = element.find('.countIssues h4').html();
    console.log(element.find('.countIssues h4').html());
    if (el > 0) {
        element.find('.showSprintModalDelete').addClass("hidden");
    } else {
        element.find('.showSprintModalDelete').removeClass("hidden");
    }
}



