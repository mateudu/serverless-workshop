module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    if (req.query.id) {
        if (req.query.id == "1") {
            context.res = {
                status: 200,
                body: {
                    id: "1",
                    flavor: "Rainbow Road",
                    "price-per-scoop": 3.99
                }
            }
        }
    }
    else {
        context.res = {
            status: 400,
            body: "Please pass in an id query paramter"
        }
    }
};