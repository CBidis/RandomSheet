(function () {
    "use strict";

    var treeviewMenu = $('.app-menu');

    // Toggle Sidebar
    $('[data-toggle="sidebar"]').click(function (event) {
        event.preventDefault();
        $('.app').toggleClass('sidenav-toggled');
    });

    $('#modal-container').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Button that triggered the modal
        var url = button.attr("href");
        var modal = $(this);

        // note that this will replace the content of modal-content ever time the modal is opened
        modal.find('.modal-content').load(url);
    });

    $('#modal-container').on('hidden.bs.modal', function () {
        // remove the bs.modal data attribute from it
        $(this).removeData('bs.modal');
        // and empty the modal-content element
        $('#modal-container .modal-content').empty();
    });

    // Activate sidebar treeview toggle
    $("[data-toggle='treeview']").click(function (event) {
        event.preventDefault();
        if (!$(this).parent().hasClass('is-expanded')) {
            treeviewMenu.find("[data-toggle='treeview']").parent().removeClass('is-expanded');
        }
        $(this).parent().toggleClass('is-expanded');
    });

    // Set initial active toggle
    $("[data-toggle='treeview.'].is-expanded").parent().toggleClass('is-expanded');

    //Activate bootstrip tooltips
    $("[data-toggle='tooltip']").tooltip();

})();

function Beautify(elementToLoad, jsonElement) {
    $(elementToLoad).jsonViewer(JSON.parse($(jsonElement).val(), { collapsed: true, withQuotes: true, withLinks: true }));
}

function ShowOrHideOnClick(elementId) {
    var element = document.getElementById(elementId);
    if (element.style.display === "none") {
        element.style.display = "block";
    } else {
        element.style.display = "none";
    }
}

function addItemToList(input, e) {
    if (e.keyCode === 13) {
        var option = new Option(input.value, input.value, false, true);
        $("#TemplateFields").append(option);
        input.value = '';
    }
}

$(document).on('submit', 'form', function (e) {

    if (e.currentTarget.id === "searchForm") {
        e.submit();
    }

    if (e.delegateTarget.activeElement.type !== "submit") {
        e.preventDefault();
    }
});


function isCollapsable(arg) {
    return arg instanceof Object && Object.keys(arg).length > 0;
}

function isUrl(string) {
    var urlRegexp = /^(https?:\/\/|ftps?:\/\/)?([a-z0-9%-]+\.){1,}([a-z0-9-]+)?(:(\d{1,5}))?(\/([a-z0-9\-._~:/?#[\]@!$&'()*+,;=%]+)?)?$/i;
    return urlRegexp.test(string);
}

function json2html(json, options) {
    var html = '';
    if (typeof json === 'string') {
        // Escape tags and quotes
        json = json
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/'/g, '&apos;')
            .replace(/"/g, '&quot;');

        if (options.withLinks && isUrl(json)) {
            html += '<a href="' + json + '" class="json-string" target="_blank">' + json + '</a>';
        } else {
            // Escape double quotes in the rendered non-URL string.
            json = json.replace(/&quot;/g, '\\&quot;');
            html += '<span class="json-string">"' + json + '"</span>';
        }
    } else if (typeof json === 'number') {
        html += '<span class="json-literal">' + json + '</span>';
    } else if (typeof json === 'boolean') {
        html += '<span class="json-literal">' + json + '</span>';
    } else if (json === null) {
        html += '<span class="json-literal">null</span>';
    } else if (json instanceof Array) {
        if (json.length > 0) {
            html += '[<ol class="json-array">';
            for (var i = 0; i < json.length; ++i) {
                html += '<li>';
                // Add toggle button if item is collapsable
                if (isCollapsable(json[i])) {
                    html += '<a href class="json-toggle"></a>';
                }
                html += json2html(json[i], options);
                // Add comma if item is not last
                if (i < json.length - 1) {
                    html += ',';
                }
                html += '</li>';
            }
            html += '</ol>]';
        } else {
            html += '[]';
        }
    } else if (typeof json === 'object') {
        var keyCount = Object.keys(json).length;
        if (keyCount > 0) {
            html += '{<ul class="json-dict">';
            for (var key in json) {
                if (Object.prototype.hasOwnProperty.call(json, key)) {
                    html += '<li>';
                    var keyRepr = options.withQuotes ?
                        '<span class="json-string">"' + key + '"</span>' : key;
                    // Add toggle button if item is collapsable
                    if (isCollapsable(json[key])) {
                        html += '<a href class="json-toggle">' + keyRepr + '</a>';
                    } else {
                        html += keyRepr;
                    }
                    html += ': ' + json2html(json[key], options);
                    // Add comma if item is not last
                    if (--keyCount > 0) {
                        html += ',';
                    }
                    html += '</li>';
                }
            }
            html += '</ul>}';
        } else {
            html += '{}';
        }
    }
    return html;
}

$.fn.jsonViewer = function (json, options) {
    // Merge user options with default options
    options = Object.assign({}, {
        collapsed: false,
        rootCollapsable: true,
        withQuotes: false,
        withLinks: true
    }, options);

    // jQuery chaining
    return this.each(function () {

        // Transform to HTML
        var html = json2html(json, options);
        if (options.rootCollapsable && isCollapsable(json)) {
            html = '<a href class="json-toggle"></a>' + html;
        }

        // Insert HTML in target DOM element
        $(this).html(html);
        $(this).addClass('json-document');

        // Bind click on toggle buttons
        $(this).off('click');
        $(this).on('click', 'a.json-toggle', function () {
            var target = $(this).toggleClass('collapsed').siblings('ul.json-dict, ol.json-array');
            target.toggle();
            if (target.is(':visible')) {
                target.siblings('.json-placeholder').remove();
            } else {
                var count = target.children('li').length;
                var placeholder = count + (count > 1 ? ' items' : ' item');
                target.after('<a href class="json-placeholder">' + placeholder + '</a>');
            }
            return false;
        });

        // Simulate click on toggle button when placeholder is clicked
        $(this).on('click', 'a.json-placeholder', function () {
            $(this).siblings('a.json-toggle').click();
            return false;
        });

        if (options.collapsed == true) {
            // Trigger click to collapse all nodes
            $(this).find('a.json-toggle').click();
        }
    });
};

//Another Lib

(function ($) {
    function encodeJSONStr(str) {
        var encodeMap = {
            '"': '\\"',
            '\\': '\\\\',
            '\b': '\\b',
            '\f': '\\f',
            '\n': '\\n',
            '\r': '\\r',
            '\t': '\\t'
        };

        return str.replace(/["\\\b\f\n\r\t]/g, function (match) {
            return encodeMap[match];
        });
    }

    function encodeJSON(json) {
        if (typeof json === 'string') {
            return encodeJSONStr(json);
        } else if (typeof json === 'object') {
            for (var attr in json) {
                json[attr] = encodeJSON(json[attr]);
            }
        } else if (Array.isArray(json)) {
            for (var i = 0; i < json.length; i++) {
                json[i] = encodeJSON(json[i]);
            }
        }

        return json;
    }

    function JsonEditor(container, json, options) {
        options = options || {};
        if (options.editable !== false) {
            options.editable = true;
        }

        this.$container = $(container);
        this.options = options;

        this.load(json);
    }

    JsonEditor.prototype = {
        constructor: JsonEditor,
        load: function (json) {
            this.$container.jsonViewer(encodeJSON(json), {
                collapsed: this.options.defaultCollapsed,
                withQuotes: true
            })
                .addClass('json-editor-blackbord')
                .attr('contenteditable', !!this.options.editable);
        },
        get: function () {
            try {
                this.$container.find('.collapsed').click();
                return JSON.parse(this.$container.text());
            } catch (ex) {
                throw new Error(ex);
            }
        }
    }

    window.JsonEditor = JsonEditor;
})(jQuery);