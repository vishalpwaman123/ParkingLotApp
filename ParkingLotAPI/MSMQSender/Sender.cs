using System;
using CommonLayer.ResponseModel;
using Experimental.System.Messaging;

namespace ParkingLotAPI.MSMQSender
{
    public class Sender
    {

        /// <summary>
        /// Define send method
        /// </summary>
        /// <param name="input">Passing input string</param>
        public void Send(String Message, string status)
        {
            try
            {
                // Created the referrence of MessageQueue
                MessageQueue messageQueue = null;

                // Created the referrence of MessageQueue
                //MessageQueue messageQueing = null;

                //if (status == "parked")
                //{
                    // Check if Message Queue Exists
                    if (MessageQueue.Exists(@".\Private$\Parkqueue"))
                    {
                        messageQueue = new MessageQueue(@".\Private$\Parkqueue");
                        messageQueue.Label = "Testing Queue";
                    }
                    else
                    {
                        MessageQueue.Create(@".\Private$\Parkqueue");
                        messageQueue = new MessageQueue(@".\Private$\Parkqueue");
                        messageQueue.Label = "Newly Created Queue";
                    }
                    // Message send to Queue
                    messageQueue.Send(Message);
                /*}
                else
                {

                    // Check if Message Queue Exists
                    if (MessageQueue.Exists(@".\Private$\UnParkqueue"))
                    {
                        messageQueing = new MessageQueue(@".\Private$\UnParkqueue");
                        messageQueing.Label = "Testing Queue";
                    }
                    else
                    {
                        MessageQueue.Create(@".\Private$\UnParkqueue");
                        messageQueing = new MessageQueue(@".\Private$\UnParkqueue");
                        messageQueing.Label = "Newly Created Queue";
                    }
                    // Message send to Queue
                    messageQueing.Send(Message);
                }*/

               
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }


    }
}
