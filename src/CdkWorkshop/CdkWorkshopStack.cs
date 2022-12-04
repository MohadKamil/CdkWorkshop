using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.SNS.Subscriptions;
using Amazon.CDK.AWS.SQS;
using Cdklabs.DynamoTableViewer;
using Constructs;

namespace CdkWorkshop
{
    public class CdkWorkshopStack : Stack
    {
        internal CdkWorkshopStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var helloFunction = new Function(this, "HelloHandler", new FunctionProps
            {
                Runtime = Runtime.NODEJS_14_X,
                Code = Code.FromAsset("lambda"),
                Handler = "hello.handler"
            });

            var helloWithHitCounter = new HitCounter(this, "HitCounterHello", new HitCounterProps
            {
                Downstream = helloFunction
            });

            var api = new LambdaRestApi(this, "RestAPI", new LambdaRestApiProps
            {
                Handler = helloWithHitCounter.Handler
            });

            var tableViewer = new TableViewer(this, "HitsTableViewer", new TableViewerProps
            {
                Title = "Hello Hits",
                Table = helloWithHitCounter.Table
            });
        }
    }
}
