import React, { Component } from 'react';
import {Comment, EnterTheRoom, HighFive, LeaveTheRoom} from "../contracts/ChatEventTypes";

export class ChatByMinute extends Component {
  static displayName = ChatByMinute.name;

  constructor(props) {
    super(props);
    this.state = { chatEvents: [], loading: true };
  }

  componentDidMount() {
    this.populateChatData();
  }

  static getDescription(chatEvent) {
    switch (chatEvent.chatEventType)
    {
      case Comment:
        return `${chatEvent.chatter} comments: "${chatEvent.text}"`;
      case HighFive:
        return `${chatEvent.chatter} high-fives ${chatEvent.chatter2}`;
      case LeaveTheRoom:
        return `${chatEvent.chatter} leaves`;
      case EnterTheRoom:
        return `${chatEvent.chatter} enters the room`;
      default:
        return "Unknown";
    }
  }
  
  static renderChatTable(chatEvents) {
    return (
      <table className='table table-striped' aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Time</th>
            <th>Description</th>
          </tr>
        </thead>
        <tbody>
          {chatEvents.map(chatEvent =>
            <tr key={chatEvent.eventDateTime}>
              <td>{chatEvent.eventDateTime}</td>
              <td>{ChatByMinute.getDescription(chatEvent)}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : ChatByMinute.renderChatTable(this.state.chatEvents);

    return (
      <div>
        <h1 id="tableLabel" >Granularity: minute by minute</h1>
        {contents}
      </div>
    );
  }

  async populateChatData() {
    const response = await fetch('chat/minute');
    const data = await response.json();
    this.setState({ chatEvents: data, loading: false });
  }
}
