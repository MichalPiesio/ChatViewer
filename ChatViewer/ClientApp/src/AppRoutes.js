import { SeedData } from "./components/SeedData";
import { ChatByMinute } from "./components/ChatByMinute";
import { ChatAggregate } from "./components/ChatAggregate";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/seed-data',
    element: <SeedData />
  },
  {
    path: '/chat-by-minute',
    element: <ChatByMinute />
  },
  {
    path: '/chat-by-hour',
    element: <ChatAggregate granularity={'hour'}/>
  },
  {
    path: '/chat-by-day',
    element: <ChatAggregate granularity={'day'}/>
  }
];

export default AppRoutes;
