using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using WmsPrism.Model.Dto;

namespace WmsPrism
{
    public class EventAggregatorRepository
    {
        public IEventAggregator eventAggregator;
        public static EventAggregatorRepository eventRepository = null;
        public EventAggregatorRepository()
        {
            eventAggregator = new EventAggregator();
        }

        public static EventAggregatorRepository GetInstance()
        {
            if (eventRepository == null)
            {
                eventRepository = new EventAggregatorRepository();
            }
            return eventRepository;
        }


        public class LoginEvent : PubSubEvent<UserDto>
        {

        }
    }
}
