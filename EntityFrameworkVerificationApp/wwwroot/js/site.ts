// Write your JavaScript code.
namespace site {
    const replyCommentIdPrefix = '#reply-comment-';
    const commentBox = '#comment-box-';
    const commentBoxClass = "comment-box";

    const addressSelectorId = "#billing-address-select";
    const addressFieldSet = "fieldset[name^=billing-address-]";
    const addressSelectedClass = "address-selected";

    export function showCommentBox(i: number) {
        const checkbox = document.querySelector(replyCommentIdPrefix + i);
        const box = document.querySelector(commentBox + i);
        checkbox.addEventListener('change', () => {
            box.classList.toggle(commentBoxClass);
        });
    }

    export function setupAddressSelector() {
        const select = document.querySelector(addressSelectorId) as HTMLSelectElement;
        select.addEventListener("change", () => {
            const index = select.selectedIndex;

            const selectedItem = select.options[index];
            const fieldSetIndex = parseInt(selectedItem.value);
            const fieldSets = document.querySelectorAll(addressFieldSet);
            for (let i = 0; i < fieldSets.length; i++) {
                const f = fieldSets[i] as HTMLFieldSetElement;
                f.classList.remove(addressSelectedClass);
                f.disabled = true;
            }

            const activeFieldSet = document.querySelector(`fieldset[name=billing-address-${fieldSetIndex}]`) as HTMLFieldSetElement;
            activeFieldSet.disabled = false;
            activeFieldSet.classList.add(addressSelectedClass);
        });
    }
}