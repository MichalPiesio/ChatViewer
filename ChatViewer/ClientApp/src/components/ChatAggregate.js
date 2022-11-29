import React, { Component } from 'react';
import { Comment, HighFive, LeaveTheRoom, EnterTheRoom} from '../contracts/ChatEventTypes';

export class ChatAggregate extends Component {
  static displayName = ChatAggregate.name;

  constructor(props) {
    super(props);
    this.state = { chatEventAggregates: [], loading: true, granularity: props.granularity };
  }

  componentDidMount() {
    this.populateChatData();
  }
  
  componentDidUpdate(prevProps, prevState, snapshot) {
    if(prevProps.granularity !== this.props.granularity)
    {
      this.state.granularity = this.props.granularity;
      this.populateChatData();
    }
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
            chatEventAggregates.map((chatEventAggregate, i) => 
              <>
                <tr key={i}>
                  <td>{chatEventAggregate.dateTime}</td>
                  <td></td>
                </tr>
                { 
                  chatEventAggregate.details.map((detail, d) => <tr key={'r'+d}>
                  <td></td>
                  <td>{ChatAggregate.getDescription(detail)}</td>
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
      : ChatAggregate.renderChatTable(this.state.chatEventAggregates);

    return (
      <div>
        <h1 id="tableLabel" >Granularity: {this.state.granularity} by {this.state.granularity}</h1>
        {contents}
      </div>
    );
  }

  async populateChatData() {
    const response = await fetch('chat/'+this.state.granularity);
    const data = await response.json();
    this.setState({ chatEventAggregates: data, loading: false });
  }
}
