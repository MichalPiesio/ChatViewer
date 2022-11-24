import { SeedData } from "./components/SeedData";
import { ChatByMinute } from "./components/ChatByMinute";
import { ChatByHour } from "./components/ChatByHour";
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
    element: <ChatByHour />
  }
];

export default AppRoutes;
