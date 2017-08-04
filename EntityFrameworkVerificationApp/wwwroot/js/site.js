// Write your JavaScript code.
var site;
(function (site) {
    var replyCommentIdPrefix = '#reply-comment-';
    var commentBox = '#comment-box-';
    var commentBoxClass = "comment-box";
    var addressSelectorId = "#billing-address-select";
    var addressFieldSet = "fieldset[name^=billing-address-]";
    var addressSelectedClass = "address-selected";
    function showCommentBox(i) {
        var checkbox = document.querySelector(replyCommentIdPrefix + i);
        var box = document.querySelector(commentBox + i);
        checkbox.addEventListener('change', function () {
            box.classList.toggle(commentBoxClass);
        });
    }
    site.showCommentBox = showCommentBox;
    function setupAddressSelector() {
        var select = document.querySelector(addressSelectorId);
        select.addEventListener("change", function () {
            var index = select.selectedIndex;
            var selectedItem = select.options[index];
            var fieldSetIndex = parseInt(selectedItem.value);
            var fieldSets = document.querySelectorAll(addressFieldSet);
            for (var i = 0; i < fieldSets.length; i++) {
                var f = fieldSets[i];
                f.classList.remove(addressSelectedClass);
                f.disabled = true;
            }
            var activeFieldSet = document.querySelector("fieldset[name=billing-address-" + fieldSetIndex + "]");
            activeFieldSet.disabled = false;
            activeFieldSet.classList.add(addressSelectedClass);
        });
    }
    site.setupAddressSelector = setupAddressSelector;
})(site || (site = {}));
//# sourceMappingURL=site.js.map