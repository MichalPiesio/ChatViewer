import React, { Component } from 'react';

export class SeedData extends Component {
  constructor(props) {
    super(props);
    this.seedData = this.seedData.bind(this);
  }

  async seedData() {
    await fetch('dataseed/seed', { method: 'POST' });
  }

    render() {
        return (
            <div>
                <p>Click the button to add more data to the database (fire and forget, don't wait for a message to pop up it completed).</p>
                <button className="btn btn-primary" onClick={this.seedData}>Seed Data</button>
            </div>
        );
    }
}
