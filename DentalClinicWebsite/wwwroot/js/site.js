let bookingMode = false;
let bookingStep = "";
let bookingData = {
    fullName: "",
    email: "",
    phone: "",
    service: "",
    preferredDate: "",
    preferredTime: ""
};

function startBookingFlow() {
    bookingMode = true;
    bookingStep = "fullName";
    appendMessage("Sure, I can help you book an appointment. What is your full name?", "bot");
}

const message = userMessageInput.value.trim();

if (!bookingMode && message.toLowerCase().includes("book")) {
    appendMessage(message, "user");
    userMessageInput.value = "";
    startBookingFlow();
    return;
}

function handleBookingStep(message) {
    if (bookingStep === "fullName") {
        bookingData.fullName = message;
        bookingStep = "email";
        appendMessage("Thanks. What is your email address?", "bot");
        return;
    }

    if (bookingStep === "email") {
        bookingData.email = message;
        bookingStep = "phone";
        appendMessage("Got it. What is your phone number?", "bot");
        return;
    }

    if (bookingStep === "phone") {
        bookingData.phone = message;
        bookingStep = "service";
        showServiceOptions();
        return;
    }

    if (bookingStep === "preferredDate") {
        bookingData.preferredDate = message;
        bookingStep = "preferredTime";
        appendMessage("What time would you prefer?", "bot");
        return;
    }

    if (bookingStep === "preferredTime") {
        bookingData.preferredTime = message;
        bookingStep = "confirm";
        showBookingConfirmation();
        return;
    }
}

function showServiceOptions() {
    appendMessage("Please choose a service:", "bot");

    const options = document.createElement("div");
    options.className = "service-options";
    options.innerHTML = `
        <button onclick="selectService('General Dentistry')">General Dentistry</button>
        <button onclick="selectService('Cosmetic Dentistry')">Cosmetic Dentistry</button>
        <button onclick="selectService('Orthodontics')">Orthodontics</button>
    `;
    chatMessages.appendChild(options);
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

function selectService(service) {
    bookingData.service = service;
    appendMessage(service, "user");
    bookingStep = "preferredDate";
    appendMessage("What date would you prefer?", "bot");
}

function showBookingConfirmation() {
    appendMessage(
        `Please confirm your booking:
Name: ${bookingData.fullName}
Email: ${bookingData.email}
Phone: ${bookingData.phone}
Service: ${bookingData.service}
Date: ${bookingData.preferredDate}
Time: ${bookingData.preferredTime}`,
        "bot"
    );

    const confirmBox = document.createElement("div");
    confirmBox.className = "service-options";
    confirmBox.innerHTML = `
        <button onclick="confirmBooking()">Confirm</button>
        <button onclick="cancelBooking()">Cancel</button>
    `;
    chatMessages.appendChild(confirmBox);
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

async function confirmBooking() {
    try {
        const response = await fetch("https://localhost:7015/api/appointments", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                fullName: bookingData.fullName,
                email: bookingData.email,
                phone: bookingData.phone,
                service: bookingData.service,
                preferredDate: bookingData.preferredDate,
                preferredTime: bookingData.preferredTime,
                message: ""
            })
        });

        if (!response.ok) {
            appendMessage("Sorry, booking failed. Please try again.", "bot");
            return;
        }

        appendMessage("Your appointment request has been submitted successfully.", "bot");

        bookingMode = false;
        bookingStep = "";
        bookingData = {
            fullName: "",
            email: "",
            phone: "",
            service: "",
            preferredDate: "",
            preferredTime: ""
        };
    } catch (error) {
        appendMessage("Sorry, something went wrong while saving your booking.", "bot");
    }
}

function cancelBooking() {
    appendMessage("Your booking request has been cancelled.", "bot");
    bookingMode = false;
    bookingStep = "";
    bookingData = {
        fullName: "",
        email: "",
        phone: "",
        service: "",
        preferredDate: "",
        preferredTime: ""
    };
}