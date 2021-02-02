using SimpleNetNlp;
using System;
using System.Text.RegularExpressions;
using Tweetinvi;
using Tweetinvi.Events;

namespace SentimentAnalysisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Authenticate
            string consumerKey = Environment.GetEnvironmentVariable("CONSUMERKEY",EnvironmentVariableTarget.Machine);
            string consumerSecret = Environment.GetEnvironmentVariable("CONSUMERSECRET", EnvironmentVariableTarget.Machine);
            string accessToken = Environment.GetEnvironmentVariable("ACCESSTOKEN", EnvironmentVariableTarget.Machine);
            string accessTokenSecret = Environment.GetEnvironmentVariable("ACCESSTOKENSECRET", EnvironmentVariableTarget.Machine);

            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            

            //Create Stream
            var stream = Stream.CreateFilteredStream();

            //Add Topic Filters
            stream.AddTrack("cryptocurrencies");
            stream.AddTrack("bitcoin");
            stream.AddTrack("ether");
            stream.AddTrack("Litecoin");

            //Filter Languages
            stream.AddTweetLanguageFilter("en");

            //Handle Matching Tweets
            stream.MatchingTweetReceived += OnMatchedTweet;
            
            //Start Stream
            stream.StartStreamMatchingAllConditions();
        }

        private static void OnMatchedTweet(object sender, MatchedTweetReceivedEventArgs args)
        {
            var sanitized = sanitize(args.Tweet.FullText); //Sanitize Tweet

            var sentence = new Sentence(sanitized);

            //Output Tweet and Sentiment
            Console.WriteLine(sentence.Sentiment + "|" + args.Tweet);
            
            //Dispose of Sentence object
            sentence = null;
        }

        private static string sanitize(string raw)
        {
            return Regex.Replace(raw, @"(@[A-Za-z0-9]+)|([^0-9A-Za-z \t])|(\w+:\/\/\S+)", "").ToString();
        }
    }
}
