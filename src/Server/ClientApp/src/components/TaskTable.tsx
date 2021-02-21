import * as React from 'react';
import * as Icons from 'react-bootstrap-icons';
import { connect } from 'react-redux';
import { RouteComponentProps } from "react-router";
import { Button, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { TodoTask } from '../model/TodoTask';
import TaskRecord from './TaskRecord';

export interface TaskTableProps {
  tasks: TodoTask[];
  newTaskId: string;
  orderByAsc: boolean;
  onToggleOrderBy: () => void;
  onNew: () => void;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
};

class TaskTable extends React.PureComponent<TaskTableProps> {
  public render() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>
              <div className="d-flex align-items-end justify-content-between no-bottom-border">
                <span>Name</span>
                <button onClick={this.props.onToggleOrderBy} className="btn">
                  {this.props.orderByAsc
                    ? (<Icons.SortAlphaDown/>)
                    : (<Icons.SortAlphaDownAlt/>)}
                </button>
              </div>
            </th>
            <th>Priority</th>
            <th>Status</th>
            <th>
              <button
                onClick={this.props.onNew}
                className="btn float-right"
              >
                <Icons.PlusCircle/>
              </button>
            </th>
          </tr>
        </thead>
        <tbody>
          {this.props.tasks
            .sort((a, b) => this.props.orderByAsc
              ? (a.name > b.name ? 1 : -1)
              : (a.name > b.name ? -1 : 1))
            .map((source: TodoTask) => 
            <TaskRecord
              key={source.id}
              task={source}
              isNew={false}
              onDelete={this.props.onDelete}
              onEdit={this.props.onEdit}
            />
          )}
        </tbody>
      </table>
    );
  }
};

export default connect()(TaskTable);