import { Injectable } from '@angular/core';
import { Actions, ofType } from '@ngrx/effects';
import { createTag } from './actions';

@Injectable()
export class TagsEffects {
    constructor(private readonly actions$: Actions) {}

    /*
    createTag$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createTag),
            switchMap(() => this.docRepository.getDocs()),
            map((docs) => rehydrateDocsSuccess({ docs }))
        )
    );
    */

    /*
    rehydrateTags$ = createEffect(() =>
        this.actions$.pipe(
            ofType(rehydrateDocs),
            switchMap(() => this.docRepository.getDocs()),
            map((docs) => rehydrateDocsSuccess({ docs }))
        )
    );
    */
}
