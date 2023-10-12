let map;

let xmlhttp = new XMLHttpRequest();
xmlhttp.open("get", "clinics/getclinics", false);
xmlhttp.send(null);
clinics = JSON.parse(xmlhttp.responseText);

function geocodeAddress(map, clinic) {
    var geocoder = new google.maps.Geocoder();
    var content = "<h5>" + clinic.Name + "</h5><hr/><p>" + clinic.Address + "</p>";
    var infoWindow = new google.maps.InfoWindow({ content: content });

    geocoder.geocode({ address: clinic.Address }, function (result, status) {
        if (status === "OK") {
            var marker = new google.maps.Marker({
                map: map,
                position: result[0].geometry.location
            });
            marker.addListener("click", function() {
                infoWindow.open(map, marker)
            })
        };
    });
}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(
        browserHasGeolocation
            ? "Error: The Geolocation service is failed"
            : "Error: Your browser doesn't support geolocation"
    );
    infoWindow.open(map);
}

function initMap() {
    // Initial Google map
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: - 37.84, lng: 145.08 },
        zoom: 10,
        zoomControl: true
    });

    // Get current location
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            position => {
                const pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                map.setCenter(pos);
                map.setZoom(15);
            },
            
            () => {
                handleLocationError(false, infoWindnow, map.getCenter())
            }
        );
    } else {
        handleLocationError(false, infoWindnow, map.getCenter())
    }

    // Mark clinics on Google map
    for (var i = 0; i < clinics.length; i++) {
        console.log(clinics[i])
        geocodeAddress(map, clinics[i])
    }

    // Auto-complete functionality
    var start = document.getElementById("start");
    const autoComplete = new google.maps.places.Autocomplete(start);
    autoComplete.bindTo("bounds", map);

    // Routing of Google map
    const directionRenderer = new google.maps.DirectionsRenderer();
    const directionsService = new google.maps.DirectionsService();
    directionRenderer.setMap(map);
    directionRenderer.setPanel(document.getElementById("sidebar"));
    var getDirection = document.getElementById("route_btn");
    getDirection.addEventListener("click", () => {
        directionsService.route({
            origin: {
                query: document.getElementById("start").value
            },
            destination: {
                query: document.getElementById("destination").value
            },
            travelMode: google.maps.TravelMode[document.getElementById("mode").value],
        }, (response, status) => {
            if (status === "OK") {
                directionRenderer.setDirections(response);
            } else {
                window.alert("Unable to get direction due to: " + status);
            }
        }
        );
    });

}
