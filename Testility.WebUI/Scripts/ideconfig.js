$(document).ready(function () {
    CodeMirror.fromTextArea($("#Code")[0], {
        matchBrackets: true,
        mode: "text/x-csharp",
        lineNumbers: true
    });
});