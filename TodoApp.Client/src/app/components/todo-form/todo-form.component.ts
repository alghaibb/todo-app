import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CreateTodoItem } from '../../models/todo-item.model';
import { TodoService } from '../../services/todo.service';

@Component({
  selector: 'app-todo-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  template: `
    <form [formGroup]="todoForm" (ngSubmit)="onSubmit()">
      <div class="form-container">
        <mat-form-field appearance="fill" class="form-field">
          <mat-label>Task</mat-label>
          <input matInput formControlName="title" placeholder="Add a new task">
          <mat-error *ngIf="todoForm.get('title')?.errors?.['required']">
            Title is required
          </mat-error>
        </mat-form-field>
        
        <button mat-raised-button color="primary" type="submit" [disabled]="todoForm.invalid">
          Add Task
        </button>
      </div>
    </form>
  `,
})

export class TodoFormComponent {
  @Output() todoAdded = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private todoService = inject(TodoService);

  todoForm: FormGroup = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(3)]]
  });

  onSubmit(): void {
    if (this.todoForm.valid) {
      const newTodo: CreateTodoItem = {
        title: this.todoForm.value.title
      };

      this.todoService.createTodo(newTodo).subscribe({
        next: () => {
          this.todoForm.reset();
          this.todoAdded.emit();
        },
        error: (error) => {
          console.error('Error adding todo:', error);
        }
      });
    }
  }
}
