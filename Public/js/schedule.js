import { times, items } from "./data.js";

$(document).ready(function () {
  let $gridContainer = $("#schedule-time");
  let $scheduleBoard = $("#schedule-board");
  let $scheduleItem = $("#schedule-item");

  // Schedule time
  const ScheduleTime = (widthClass) => {
    times.forEach(function (time) {
      const $timeDiv = $("<div></div>", {
        class:
          "col-start-1 relative after:content-[''] after:absolute after:border-b after:h-1 flex justify-center items-center after:border-gray-200 after:bottom-0 z-20" +
          " " +
          widthClass,
        text: time,
      });
      $gridContainer.append($timeDiv);
    });
  };

  const AddScheduleTime = () => {
    let scheduleBoardWidth;
    if ($(window).width() < 500) {
      scheduleBoardWidth = $scheduleBoard.width();
    } else {
      scheduleBoardWidth = ($scheduleBoard.width() * 27) / 32;
    }
    const widthTemplate =
      $(window).width() < 500
        ? `after:left-0 after:w-[${scheduleBoardWidth}px]`
        : `after:left-1/3 after:w-[${scheduleBoardWidth}px]`;

    $gridContainer.empty();

    ScheduleTime(widthTemplate);
  };

  // Schedule item
  const ScheduleItem = (props) => {
    let $item;
    if (props.type == "avalaible") {
      $item = $("<div></div>", {
        class: `avalaible schedule-item col-start-${props.colStart} row-span-${props.rowSpan} row-start-${props.rowStart} z-30 opacity-60 rounded-xl font-semibold text-sm flex justify-center items-center my-2`,
        text: props.text,
      });
    } else if (props.type == "activate") {
      $item = $("<div></div>", {
        class: `activate schedule-item col-start-${props.colStart} z-30 opacity-60 rounded-xl font-semibold flex justify-center items-center text-xs my-2`,
        text: props.text,
      });
    } else {
      $item = $("<div></div>", {
        class: `unavalaible schedule-item col-start-${props.colStart} z-30 opacity-60 rounded-xl font-semibold flex justify-center items-center text-xs my-2`,
        text: props.text,
      });
    }

    $item.on("click", function () {
      $("#popup-container").load("/components/popup/info.html", function () {
        // Update the content with item properties
        $("#popup-container").find(".time-start").text(props.timeStart);
        $("#popup-container").find(".time-end").text(props.timeEnd);
        $("#popup-container").find(".item-text").text(props.text);
          $("#popup-container").find(".item-room").text(props.room);
          $("#popup-container").find(".item-pax").text(props.pax);
      });
    });

    $(document).on("click", ".button-back", function () {
      $("#popup-container").empty(); // Remove the popup content
      $("#popup").addClass("hidden"); // Hide the popup
    });

    $(document).on("click", ".button-submit", function () {
      $("#popup-container").load("/components/popup/submit.html", function () {
        // Update the content with item properties
        $("#popup-container").find(".time-start").text(props.timeStart);
        $("#popup-container").find(".time-end").text(props.timeEnd);
        $("#popup-container").find(".item-text").text(props.text);
        
      });
    });
    

    return $item;
  };

  const AddScheduleItem = () => {
    items.forEach((item) => {
      $scheduleItem.append(ScheduleItem(item));
    });
  };

  const createLiveTimeBar = () => {
    const $scheduleTimeDiv = $("#schedule-time");
    let scheduleWidth = $scheduleBoard.width();
    // Create the live time bar if it doesn't exist
    let $liveTimeBar = $("#live-time-bar");
    if ($liveTimeBar.length === 0) {
      $liveTimeBar = $("<div>", {
        id: "live-time-bar",
        class:
          "absolute h-1 bg-secondary z-10" + " " + `w-[${scheduleWidth}px]`,
      });
      $scheduleTimeDiv.append($liveTimeBar);
    }

    const updateLiveTimeBar = () => {
      const now = new Date();
      const startTime = new Date();
      startTime.setHours(8, 45, 0); // 8:45 AM
      const endTime = new Date();
      endTime.setHours(15, 45, 0); // 3:45 PM

      if (now >= startTime && now <= endTime) {
        const totalMinutes = (endTime - startTime) / 60000; // Total minutes in the schedule
        const elapsedMinutes = (now - startTime) / 60000; // Minutes elapsed since start time
        const percentage = (elapsedMinutes / totalMinutes) * 100;

        $liveTimeBar.css("top", `${percentage}%`);
        $liveTimeBar.show();
      } else {
        $liveTimeBar.hide(); // Hide the bar if out of schedule time
      }
    };

    // Update the live time bar position every minute
    setInterval(updateLiveTimeBar, 60000);
    updateLiveTimeBar(); // Initial call to set the position
  };

  AddScheduleTime();
  AddScheduleItem();
  const updateWidthClass = () => {
    AddScheduleTime();
  };

  $(window).resize(updateWidthClass);
  createLiveTimeBar();
});
