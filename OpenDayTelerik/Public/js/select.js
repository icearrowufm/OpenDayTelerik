// $(document).ready(function () {
//   function loadPopup(props, component) {
//     $("#popup-container").load(component, function () {
//       lucide.createIcons();

//       $("#select-title").text(props.selectTitle);

//       // Set initial state from props
//       if (props.selectedText) {
//         $("#selected-text").text(props.selectedText);
//       }

//       $("#open-popup").click(function () {
//         $("#popup-overlay").removeClass("hidden");
//       });

//       $("#close-popup").click(function () {
//         $("#popup-overlay").addClass("hidden");
//       });

//       $("#select-who").click(function () {
//         $("#select-who-dropdown").toggleClass("expanded");
//         $("#select-who").toggleClass("border-gray-400 border-secondary");
//       });

//       const optionsContainer = $("#select-who-dropdown .w-full");
//       props.options.forEach((option) => {
//         const optionHtml = `<div class="py-2 px-2 text-darkneutral hover:bg-gray-200 option">${option}</div>`;
//         optionsContainer.append(optionHtml);
//       });

//       $(".option").click(function () {
//         const selectedText = $(this).text();
//         $("#selected-text").text(selectedText);
//         props.selectedState = selectedText;
//         $("#select-who-dropdown").removeClass("expanded");
//         $("#select-who").toggleClass("border-gray-400 border-secondary");
//       });
//     });
//   }

//   // Example
//   const propsWho = {
//     selectTitle: "Bạn là?",
//     selectedText: "Học sinh",
//     options: ["Học sinh", "Phụ Huynh"],
//   };

//   const propsClass = {
//     selectTitle: "x:xx - x:xx",
//     selectedText: "Pyschology",
//     options: [
//       "Physchology",
//       "Art & Media Studies",
//       "Econ",
//       "History",
//       "Business",
//     ],
//   };

//   const addOnClick = () => {
//     document.querySelectorAll(".schedule-item").forEach((item) => {
//       item.addEventListener("click", () => {
//         loadPopup(propsClass, "components/popup/select.html");
//       });
//     });
//   };
//   // Usage
//   loadPopup(propsWho, "components/popup/select.html");
//   addOnClick();
// });
