import {Products, ProductsResponse} from 'src/app/models/products.model';


export interface ProductsState {
  productList: Products[];
  productResponse: ProductsResponse;
  isLoading: boolean;
  isSuccess: boolean;
  error: any;
  isLoadingdel: boolean;
  isSuccessdel: boolean;
  isLoadinngAdd: boolean;
  isSuccessAdd: boolean;
  isUpSuccess: boolean;
  isUpLoading: boolean;
}

