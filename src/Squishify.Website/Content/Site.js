﻿/// <reference path="~/Scripts/jquery-1.8.0.js" />
/// <reference path="~/Scripts/json2.js" />

$(function () {
    var originalSize = 0;
    var resultTypes = {};

    $('#app-info').corner('top 15px');
    $('#main-content').corner('20px');
    $('textarea').corner('5px');
    $('.type-list-item').corner('8px');

    var winHeight = $(window).height() - 450;
    if (winHeight < 150) {
        winHeight = 150;
    }
    $('textarea').height(winHeight);

    $('#source-text').select();

    var sendRequest = function (button, url) {
        var source = $('#source-text').val();
        var minifier = $('#minifier option:selected').val();
        var action = $('#action').val() || 'js';

        var data = JSON.stringify({ source: source, minifier: minifier });

        $.ajax({
            url: 'api/' + action,
            type: 'POST',
            contentType: "application/json",
            data: data,
            dataType: 'json',

            beforeSend: function (x) {
                if (x && x.overrideMimeType) {
                    x.overrideMimeType("application/j-son;charset=UTF-8");
                }
            },
            success: function (data) {
                originalSize = data.originalSize;

                var $types = $('#minifier-types');
                $types.empty();
                for (var i = 0; i < data.types.length; i++) {
                    var type = data.types[i];
                    var id = type.id;
                    var title = type.id + ' (' + (type.difference || 0) + '%)';
                    resultTypes[id] = type;

                    $types.append('<a href="#" data-type-id="' + id + '">' + title + '</a>');
                }

                setSelectedType(data.smallest);

                $('html,body').animate({
                    scrollTop: $('#result-text').offset().top
                }, 'slow');
            },
            error: function () {
                $('#result-text').val('');
                setStats('-', '-', '-');
                $('#source-text').addClass('error');

                setUiDone();
            }
        });
    };

    var setStats = function (orgSize, minSize, difference) {
        $('#org-size span').text(orgSize);
        $('#min-size span').text(minSize);
        $('#difference span').text(difference);
    };

    var setUiLoading = function () {
        $('#minify, #source-text').attr('disbled', 'disabled');
        $('#minify').parent().addClass("loading");
    };

    var setUiDone = function () {
        $('#minify, #source-text').removeAttr("disabled");
        $('#minify').parent().removeClass("loading");
    };

    $('#minify').click(function () {
        setUiLoading();

        $('#source-text').removeClass('error');
        sendRequest(this);
    });

    var setSelectedType = function (typeNameId) {
        $('#minifier-types a').removeClass('selected');
        $('#minifier-types a[data-type-id="' + typeNameId + '"]').addClass('selected');

        var resultType = resultTypes[typeNameId];

        $('#result-text').val(resultType.minifiedContent || '');
        setStats(originalSize, resultType.minifiedSize, (resultType.difference || 0) + '%');

        setUiDone();
    }

    $('#minifier-types').on('click', 'a', function (e) {
        e.preventDefault();

        var type = $(this).attr('data-type-id');
        setSelectedType(type);
    });
});