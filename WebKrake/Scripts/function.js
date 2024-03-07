function getCookie(name) {
    var dc = document.cookie;
    var prefix = name + "=";
    var begin = dc.indexOf("; " + prefix);
    if (begin == -1) {
        begin = dc.indexOf(prefix);
        if (begin != 0) return null;
    }
    else {
        begin += 2;
        var end = document.cookie.indexOf(";", begin);
        if (end == -1) {
            end = dc.length;
        }
    }
    // because unescape has been deprecated, replaced with decodeURI
    //return unescape(dc.substring(begin + prefix.length, end));
    return decodeURI(dc.substring(begin + prefix.length, end));
} 

/* When the user scrolls down, hide the navbar. When the user scrolls up, show the navbar */
var prevScrollpos = window.pageYOffset;
window.onscroll = function () {
    var currentScrollPos = window.pageYOffset;
    if (100 > currentScrollPos) {
        document.getElementById("navbar-header").style.display = "inline-block";
        document.getElementById("dropdown-login").style.display = "block";
    } else {
        document.getElementById("navbar-header").style.display = "none";
        document.getElementById("dropdown-login").style.display = "none";
    }
    prevScrollpos = currentScrollPos;
} 

function cookie() {
    var cookie = getCookie("CookieLawAccepted");
    if (cookie == null) {
        document.getElementById("dropdown-cookies").classList.add("cookie");
    }
}
function hideCookie() {
    document.getElementById("dropdown-cookies").classList.remove("cookie");
    document.cookie = "CookieLawAccepted=true";
}


function showObject(id) {
    if (document.getElementById(id).style.display == "block") {
        document.getElementById(id).style.display = "none";
    } else {
        document.getElementById(id).style.display = "block";
    }
    
}


function checkBox(id) {
    var checkBox = "sponsored-checkbox-" + id;
    var label = "sponsored-label-" + id;
    if (id == "party" || id == "bar") {
        if (document.getElementById(checkBox).checked) {
            document.getElementById(label).innerHTML = "Gesponsert!";
            document.getElementById(label).style.color = "#f60";
        } else {
            document.getElementById(label).innerHTML = "Gesponsert?"
            document.getElementById(label).style.color = "#333";
        }
    } else {
        if (document.getElementById(checkBox).checked) {
            document.getElementById(label).innerHTML = "Die Location ist eine Bar!";
            document.getElementById(label).style.color = "#f60";
        } else {
            document.getElementById(label).innerHTML = "Ist die Location eine Bar!?"
            document.getElementById(label).style.color = "#333";
        }
    }
}

var lastTab = "Party";
function openTab(evt, cityName) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    if (cityName == "Party") {
        if (lastTab == "Bar") {
            document.getElementById("Party").className = document.getElementById("Party").className.replace(" SwipeRight", "");
            document.getElementById("Party").className = document.getElementById("Party").className.replace(" SwipeLeft", "");
            document.getElementById("Party").className += " SwipeLeft";
        } else if (lastTab = "Location") {
            document.getElementById("Party").className = document.getElementById("Party").className.replace(" SwipeRight", "");
            document.getElementById("Party").className = document.getElementById("Party").className.replace(" SwipeLeft", "");
            document.getElementById("Party").className += " SwipeRight";
        }
    }
    

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
    if (cityName == "Delete") {
        document.getElementById("create-img-selection").style.display = "none";
        clearInput();
    } else {
        document.getElementById("create-img-selection").style.display = "block";
    }
    lastTab = cityName;
    clearInput();
}

function showFullLocation(idRow, idGrid) {
    document.getElementById(idRow).classList.toggle("expanded");
    document.getElementById(idGrid).classList.toggle("expandedEvent");    
}
function showFullEvent(id) {
    document.getElementById(id).classList.toggle("expandedEvent");
}

function clickedOnImage(id) {
    document.getElementById("create-img-bar").value = "https://www.krake-party.de/img/example_" + id + ".jpg";
    document.getElementById("create-img-party").value = "https://www.krake-party.de/img/example_" + id + ".jpg";
    document.getElementById("create-img-location").value = "https://www.krake-party.de/img/example_" + id + ".jpg";
}

function addShake(id) {
    document.getElementById(id).classList.add("shake");
}

function clearInput() {
    document.getElementById("create-img-bar").value = "";
    document.getElementById("create-img-party").value = "";
    document.getElementById("create-img-location").value = "";
    document.getElementById("delte-obj-name").value = "";
    document.getElementById("delte-obj-des").value = "";
    document.getElementById("delte-obj-date").value = "";
    document.getElementById("delte-obj-time").value = "";
    document.getElementById("delte-obj-type").value = "";
}

function selectDelteObj(id, name, des, date, time, type) {
    document.getElementById("delte-obj-id").value = id;
    document.getElementById("delte-obj-name").value = name;
    document.getElementById("delte-obj-des").value = des;
    document.getElementById("delte-obj-date").value = date;
    document.getElementById("delte-obj-time").value = time;
    document.getElementById("delte-obj-type").value = type;
}
