import React, { Component } from 'react';
import { Comment, HighFive, LeaveTheRoom, EnterTheRoom} from '../contracts/ChatEventTypes';

export class ChatByHour extends Component {
  static displayName = ChatByHour.name;

  constructor(props) {
    super(props);
    this.state = { chatEventAggregates: [], loading: true };
  }

  componentDidMount() {
    this.populateChatData();
  }
  
  static getDescription(detail) {
    switch (detail.chatEventType)
    {
      case Comment:
        return `${detail.count1} ${detail.count1 === 1 ? 'comment' : 'comments'}`;
      case HighFive:
        return `${detail.count1} ${detail.count1 === 1 ? 'person' : 'people'} high-fived ${detail.count2} other ${detail.count2 === 1 ? 'person' : 'people'}`;
      case LeaveTheRoom:
        return `${detail.count1} ${detail.count1 === 1 ? 'person' : 'people'} left`;
      case EnterTheRoom:
        return `${detail.count1} ${detail.count1 === 1 ? 'person' : 'people'} entered`;
      default:
        return "Unknown";
    }
  }
  
  static renderChatTable(chatEventAggregates) {
    return (
      <table className='table table-striped' aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Time</th>
            <th>Description</th>
          </tr>
        </thead>
        <tbody>
          {
            chatEventAggregates.map(chatEventAggregate => 
              <>
                <tr key={chatEventAggregate.dateTime.toString()}>
                  <td>{chatEventAggregate.dateTime}</td>
                  <td></td>
                </tr>
                { 
                  chatEventAggregate.details.map(detail => <tr key={chatEventAggregate.dateTime.toString() + detail.chatEventType}>
                  <td></td>
                  <td>{ChatByHour.getDescription(detail)}</td>
                  </tr>)
                }
              </>)
          }
        </tbody>
      </table>
    );
  }
  
  
  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : ChatByHour.renderChatTable(this.state.chatEventAggregates);

    return (
      <div>
        <h1 id="tableLabel" >Granularity: hour by hour</h1>
        {contents}
      </div>
    );
  }

  async populateChatData() {
    const response = await fetch('chat/hour');
    const data = await response.json();
    console.log(data);
    this.setState({ chatEventAggregates: data, loading: false });
  }
}
