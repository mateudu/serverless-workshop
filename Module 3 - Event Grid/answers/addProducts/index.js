module.exports = async function (context, req) {
    context.log('Add product processed a request.');

    context.bindings.product = req.body;
};