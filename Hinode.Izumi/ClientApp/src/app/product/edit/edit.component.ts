import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ProductService, ProductWebModel } from '../../shared/web.api.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit, OnDestroy {
  private _destroyed: Subject<unknown>;

  form: FormGroup;
  isSaving: boolean;

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _productService: ProductService) {
    this._destroyed = new Subject();
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      name: [null, Validators.required],
      price: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: ProductWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new ProductWebModel();
        model.init({id: 0});
      }

      this.form.patchValue(model);
    });
  }

  ngOnDestroy(): void {
    this._destroyed.next();
    this._destroyed.complete();
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const model = new ProductWebModel(this.form.value);

    if (model.price < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Price must be greater then 0!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._productService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/fish/edit/${x.id}`);
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Product added!'});
          });
    }
    else {
      model.updatedAt = new Date();
      this._productService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Product updated!'});
          });
    }
  }

}
