module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    context.res = {
        status: 200,
        body: {
            id: "1",
            flavor: "Rainbow Road",
            "price-per-scoop": 3.99
        }
    }
};