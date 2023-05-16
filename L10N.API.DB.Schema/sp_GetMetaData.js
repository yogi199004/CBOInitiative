function sample(prefix) {

    var collection = getContext().getCollection();



    // Query documents and take 1st item.

    var isAccepted = collection.queryDocuments(

        collection.getSelfLink(),

        'SELECT value r.MetaData FROM root r',

        function (err, feed, options) {

            if (err) throw err;



            // Check the feed and if empty, set the body to 'no docs found', 

            // else take 1st element from feed

            if (!feed || !feed.length) {

                var hasData = false;

                var response = getContext().getResponse();

                var body = { hasData, feed: 'no docs found' };

                response.setBody(body);

            }

            else {

                var hasData = true;

                var response = getContext().getResponse();

                var body = { hasData, MetaData: feed[0] };

                response.setBody(body);

            }

        });



    if (!isAccepted) throw new Error('The query was not accepted by the server.');

}