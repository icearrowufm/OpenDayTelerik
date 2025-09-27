import { select1, select2 } from "/js/data.js";

$(document).ready(function () {
  // Append options to #select1
  select1.forEach((option) => {
    const $div = $("<div></div>", {
      class: "options py-2 px-2 hover:bg-neutral",
      text: option,
    });
    $("#select1").append($div);
  });

  // Append options to #select2
  select2.forEach((option) => {
    const $div = $("<div></div>", {
      class: "options py-2 px-2 hover:bg-neutral",
      text: option,
    });
    $("#select2").append($div);
  });

  // Add click event to .select-view
  $(".select-view").on("click", function () {
    const $selectDropdown = $(this).next(".select-dropdown");
    $selectDropdown.toggleClass("expanded");
    $(this).find(".focus-line").addClass("bg-secondary");
  });

  // Add click event to .options
  $(document).on("click", ".options", function () {
    const selectedText = $(this).text();
    const $selectView = $(this)
      .closest(".select-dropdown")
      .prev(".select-view");
    $selectView.find("input").val(selectedText);
    $(this).closest(".select-dropdown").removeClass("expanded");
    $(this)
      .closest(".select-dropdown")
      .next(".text-red-500")
      .addClass("hidden"); // Hide error message when an option is selected
    $selectView
      .find(".focus-line")
      .removeClass("bg-secondary bg-red-500")
      .addClass("bg-neutral");
  });

  // Add focus and blur events to inputs
  $("input")
    .on("focus", function () {
      $(this).next(".focus-line").addClass("bg-secondary");
    })
    .on("blur", function () {
      $(this).next(".focus-line").removeClass("bg-secondary");
    });

  // Add blur event to inputs
  $("input[required]").on("blur", function () {
    const $input = $(this);
    const value = $input.val();
    const type = $input.attr("type");
    let isValid = true;
    let errorMessage = "Trường này là bắt buộc.";

    if (!value) {
      isValid = false;
    } else if (type === "email" && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      isValid = false;
      errorMessage = "Email không hợp lệ.";
    } else if (type === "tel" && !/^\d{10,11}$/.test(value)) {
      isValid = false;
      errorMessage = "Số điện thoại không hợp lệ.";
    }

    if (!isValid) {
      $input
        .next(".focus-line")
        .removeClass("bg-neutral")
        .addClass("bg-red-500");
      $input.nextAll(".text-red-500").text(errorMessage).removeClass("hidden");
    } else {
      $input
        .next(".focus-line")
        .removeClass("bg-red-500")
        .addClass("bg-neutral");
      $input.nextAll(".text-red-500").addClass("hidden");
    }
  });

  // Add blur and change events to select inputs
  $("#select1-input, #search-input").on("blur change", function () {
    const inputValue = $(this).val().toLowerCase();
    const selectId =
      $(this).attr("id") === "select1-input" ? "select1" : "select2";
    const isValid = (selectId === "select1" ? select1 : select2).some(
      (option) => option.toLowerCase() === inputValue
    );

    if (!isValid) {
      $(this)
        .closest(".select-view")
        .nextAll(".text-red-500")
        .removeClass("hidden");
      $(this)
        .closest(".select-view")
        .find(".focus-line")
        .removeClass("bg-neutral")
        .addClass("bg-red-500");
    } else {
      $(this)
        .closest(".select-view")
        .nextAll(".text-red-500")
        .addClass("hidden");
      $(this)
        .closest(".select-view")
        .find(".focus-line")
        .removeClass("bg-red-500")
        .addClass("bg-neutral");
    }

    // Hide dropdown when input loses focus
    const $selectDropdown = $(this)
      .closest(".select-view")
      .next(".select-dropdown");
    $selectDropdown.removeClass("expanded");
    $(this)
      .closest(".select-view")
      .find(".focus-line")
      .removeClass("bg-secondary");
  });

  // Add blur and focus events to select elements
  const selectElements = document.querySelectorAll("select");

  selectElements.forEach((selectElement) => {
    selectElement.addEventListener("blur", () => {
      if (selectElement.value) {
        selectElement.classList.remove("bg-red-500");
        selectElement.classList.add("bg-neutral");
      } else {
        selectElement.classList.remove("bg-neutral");
        selectElement.classList.add("bg-red-500");
      }
    });

    selectElement.addEventListener("change", () => {
      if (selectElement.value) {
        selectElement.classList.remove("bg-red-500");
        selectElement.classList.add("bg-neutral");
      } else {
        selectElement.classList.remove("bg-neutral");
        selectElement.classList.add("bg-red-500");
      }
    });
  });
});
