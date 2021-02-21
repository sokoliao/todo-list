import * as React from 'react';
import { connect } from 'react-redux';
import { Route, RouteComponentProps } from 'react-router';
import { Button, Form, FormGroup, FormText, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { ApplicationState } from '../store';
import * as TodoListStore from '../store/TodoList';
import DeleteTaskDialog from './DeleteTaskDialog';
import ErrorPanel from './ErrorPanel';
import ModalTask from './ModalTask';
import NewTask from './ModalTask';
import TaskRecord from './TaskRecord';
import TaskTable from './TaskTable';

type TodoListProps =
  TodoListStore.TodoListState
  & typeof TodoListStore.actionCreators
  & RouteComponentProps<{}>

class TodoList extends React.PureComponent<TodoListProps> {
  public componentDidMount() {
    this.ensureDataFetched();
  }

  public componentDidUpdate() {
    // this.ensureDataFetched();
  }

  public render() {
    return (
      <React.Fragment>
        <ErrorPanel 
          isOpen={this.props.isErrorDisplayed}
          message={this.props.errorMessage}
          onClose={this.props.closeErrorPanel}
        />
        <TaskTable
          tasks={this.props.tasks}
          newTaskId={this.props.newTask.id}
          onNew={this.props.toggleNewTaskMenu}
          onEdit={id => this.props.toggleTaskEditMenu(id)}
          onDelete={id => this.props.toggleDeleteDialog(id)}
          onToggleOrderBy={this.props.toggleOrderBy}
          orderByAsc={this.props.isOrderByAsc}
        />
        <ModalTask
          title="Create new task"
          isOpen={this.props.isNewTaskMenuOpen}
          toggle={this.props.toggleNewTaskMenu}
          onChange={this.props.handleNewTaskChange}
          model={this.props.newTask}
          onSubmit={this.props.handleSubmit}
          validation={this.props.newTaskValidation}
        />
        <ModalTask
          title="Edit"
          isOpen={this.props.isEditTaskMenuOpen}
          toggle={() => this.props.toggleTaskEditMenu(this.props.taskInEdit.id)}
          onChange={this.props.handleTaskEdit}
          model={this.props.taskInEdit}
          onSubmit={this.props.handleTaskEditSubmit}
          validation={this.props.editTaskValidation}
        />
        <DeleteTaskDialog 
          isOpen={this.props.isDeleteDialogOpen}
          toggle={() => this.props.toggleDeleteDialog(this.props.taskToDelete.id)}
          model={this.props.taskToDelete}
          onSubmit={this.props.handleTaskDeleteSubmit}
        />
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestAllTodoTasks();
  }
}

export default connect(
  (state: ApplicationState) => state.todoList,
  TodoListStore.actionCreators
)(TodoList as any);