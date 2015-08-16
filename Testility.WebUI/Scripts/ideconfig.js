$(document).ready(function () {
    CodeMirror.fromTextArea($("#Items_0_Code")[0], {
        matchBrackets: true,
        mode: "text/x-csharp",
        lineNumbers: true
    });
});