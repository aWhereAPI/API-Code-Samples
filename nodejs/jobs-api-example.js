var https = require('https');

// request encapsulates common request functionality allowing
// us to perform web request in a common way.
//
// options: https options used to make the request.
// body:    the body, if any, to be written in the request.
const request = function (options, body) {
    return new Promise((resolve, reject) => {
        const request = https.request(options, (response) => {
            if (response.statusCode < 200 || response.statusCode > 299) {
                reject(new Error('Failed to load page, status code: ' + response.statusCode));
            }
            response.body = []
            response.on('data', (chunk) => response.body.push(chunk));
            response.on('end', () => {
                response.body = JSON.parse(response.body);
                resolve(response);
            });
        });
        request.on('error', (err) => reject(err));
        if (body != undefined) {
            console.log("writing the body...");
            request.write(body);
        }
        request.end();
    })
};

///////////////////////////////////////////////////////////////////////////////
// aWhere Jobs Request
// 
// Please see https://developer.awhere.com/api/reference/batch/create for 
// additional information.
///////////////////////////////////////////////////////////////////////////////

// getJobOptions returns the options used to make a jobs API request
const getJobOptions = function (verb, location) {
    return {
        method: verb,
        host: 'api.awhere.com',
        path: location,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + auth.token
        }
    }
}

// Here we define the job request body; all of the requests are added to the requests array
// so that we may make a batch request with them below.  You would have to set the field
// IDs (1, 2, 3) to actual field IDs that you've defined in order for this example to
// work.
var jobBody = {
    "title": "Yesterdays Weather, All Fields",
    "type": "batch",
    "requests": [
        {
            "title": "Field 1 Weather",
            "api": "GET /v2/weather/fields/1/observations/2017-02-01?properties=temperatures,precipitation"
        },
        {
            "title": "Field 2 Weather",
            "api": "GET /v2/weather/fields/2/observations/2017-02-01?properties=temperatures,precipitation"
        },
        {
            "title": "Field 3 Weather",
            "api": "GET /v2/weather/fields/3/observations/2017-02-01?properties=temperatures,precipitation"
        }
    ]
};

const getJobResults = function (location) {
    var results = null;
    request(getJobOptions('GET', location))
        .then(function (response) {
            ///////////////////////////////////////////////////////////////////
            // Results: this is what we were after in this example.
            ///////////////////////////////////////////////////////////////////
            console.log(response.body);
        })
        .catch((err) => console.error(err));
};

// printJobsResponse logs to the console, the output of each batch job in the response.
var printJobsResponse = function (response) {
    // Here we get the result of the POST and print both 
    // important pieces of information: location & body
    console.log(response.headers.location);
    console.log(response.body);

    // here we can poll the location sent back in the header
    // for the result set using timeouts or intervals.  We
    // know this opertaion will complete in a few seconds so
    // we set an appropriate interval.
    var interval = setInterval(() => {
        request(getJobOptions('GET', response.headers.location))
            .then(function (response) {
                ///////////////////////////////////////////////////////////////////
                // Results: this is what we were after in this example.
                ///////////////////////////////////////////////////////////////////
                var job = response.body;
                if (job.results) {
                    clearInterval(interval);
                    for (var i = 0; i < job.results.length; i++) {
                        console.log("\n==================================================================================");
                        console.log(" "+job.results[i].title);
                        console.log("==================================================================================");
                        console.log("api:    "+job.results[i].api);
                        console.log("status: "+job.results[i].httpStatus);
                        console.log("payload: \n"+JSON.stringify(job.results[i].payload));
                    } 
                }
            })
            .catch((err) => console.error(err));
    }, 2000);
}

///////////////////////////////////////////////////////////////////////////////
// Authentication
///////////////////////////////////////////////////////////////////////////////

// IMPORTANT: The key and secret below must be set to your key and secret for 
// this example to work correctly.
var auth = {
    key: 'KeyKeyKeyKey',
    secret: 'SecretSecretSecretSecret',
    base64: null,
    token: null
}

// Build base64 encoded key:secret combination
if (auth.base64 == undefined) {
    auth.base64 = new Buffer(auth.key + ":" + auth.secret).toString('base64');
    console.log(auth.base64);
}

// auth request options
var authOptions = {
    method: 'POST',
    host: 'api.awhere.com',
    path: '/oauth/token',
    headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        'Authorization': 'Basic ' + auth.base64
    }
};

// Aquire a token if not provided...
var authPromise = null;
if (auth.token == undefined) {
    authPromise = request(authOptions, "grant_type=client_credentials");
} else {
    authPromise = new Promise((resolve, reject) => { resolve() });
}

// We are done defining functionality, so we can handle the authPromise
// and make other requests as necessary. 
authPromise
    .then(function (response) {
        // Set the auth.token if it is returned
        if (response && response.body) {
            auth.token = JSON.parse(response.body).access_token;
        }
    }).then(function () {
        // once we have the auth token, we can make the jobs request.
        request(getJobOptions('POST', '/v2/jobs'), JSON.stringify(jobBody))
            .then(function (response) {
                printJobsResponse(response);
            })
            .catch((err) => console.error(err));
    })
    .catch((err) => console.error(err));
